using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    public GameManager gameManager;
    public GameState currentState;
    public GameState destinationState;
    public bool AITurn = false;
    public int farsightVal = 3;
    GameState maxEval;
    GameState minEval;
    List<GameState> lastBranchParents = new List<GameState>();

    private void Start()
    {
        currentState = gameManager.gameState;

    }
    public GameState MinMax(GameState position, int depth, bool maximisingPlayer, int maxDepth)
    {
        GameState nextState;
        foreach (GameState child in position.GetPossibleStates())
        {
            lastBranchParents.Add(child.FindLastStateParent(depth, maxDepth));
        }
        if (lastBranchParents.Count != 0)
        {
            foreach (GameState finalParent in lastBranchParents)
            {
                finalParent.FindMinMaxState(finalParent.GetPossibleStates(), maximisingPlayer).SetParentChainEval();
            }
        }
        nextState = position.FindMinMaxState(position.GetPossibleStates(), maximisingPlayer);
        return nextState;
    }
}
