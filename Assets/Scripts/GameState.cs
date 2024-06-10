using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using static Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetrics;

public class GameState
{
    int value;
    int[,] board = new int[12, 12]
        {   {   1000, 1000, 1000, 1000, 1000, 1000, 1000, 1000, 1000, 1000, 1000, 1000 },
            {   1000, 1000, 1000, 1000, 1000, 1000, 1000, 1000, 1000, 1000, 1000, 1000 },
            {   1000, 1000, 0, 0, 0, 0, 0, 0, 0, 0, 1000, 1000 },
            {   1000, 1000, 0, 0, 0, 0, 0, 0, 0, 0, 1000, 1000 },
            {   1000, 1000, 0, 0, 0, 0, 0, 0, 0, 0, 1000, 1000 },
            {   1000, 1000, 0, 0, 0, 0, 0, 0, 0, 0, 1000, 1000 },
            {   1000, 1000, 1, 0, 0, 0, 0, 0, 0, -1, 1000, 1000 },
            {   1000, 1000, 1, 0, 0, 0, 0, 0, 0, -1, 1000, 1000 },
            {   1000, 1000, 1, 0, 0, 0, 0, 0, 0, -1, 1000, 1000 },
            {   1000, 1000, 1, 0, 0, 0, 0, 0, 0, -1, 1000, 1000 },
            {   1000, 1000, 1000, 1000, 1000, 1000, 1000, 1000, 1000, 1000, 1000, 1000 },
            {   1000, 1000, 1000, 1000, 1000, 1000, 1000, 1000, 1000, 1000, 1000, 1000 }
        };
    int[,] p1Pieces = new int[12, 12];
    int[,] p2Pieces = new int[12, 12];
    List<GameState> PossibleStates = new List<GameState>();
    GameState ParentState;
    bool p1Turn = false;
    int currentDepth;
    GameState maxState;
    GameState minState;
    enum MoveDirections
    {
        up,
        down,
        left,
        right
    }
    public GameState(bool turn, GameState ParentState = null, int[,] board = null)
    {
        this.p1Turn = turn;
        if (board != null)
        {
            for (int i = 0; i < board.GetLength(0); i++)
            {
                for (int j = 0; j < board.GetLength(1); j++)
                {
                    this.board[i, j] = board[i, j];
                }
            }
        }
        else
        {
            this.board = new int[12, 12]
            {   {   1000, 1000, 1000, 1000, 1000, 1000, 1000, 1000, 1000, 1000, 1000, 1000 },
                {   1000, 1000, 1000, 1000, 1000, 1000, 1000, 1000, 1000, 1000, 1000, 1000 },
                {   1000, 1000, 0, 0, 0, 0, 0, 0, 0, 0, 1000, 1000 },
                {   1000, 1000, 0, 0, 0, 0, 0, 0, 0, 0, 1000, 1000 },
                {   1000, 1000, 0, 0, 0, 0, 0, 0, 0, 0, 1000, 1000 },
                {   1000, 1000, 0, 0, 0, 0, 0, 0, 0, 0, 1000, 1000 },
                {   1000, 1000, 1, 0, 0, 0, 0, 0, 0, -1, 1000, 1000 },
                {   1000, 1000, 1, 0, 0, 0, 0, 0, 0, -1, 1000, 1000 },
                {   1000, 1000, 1, 0, 0, 0, 0, 0, 0, -1, 1000, 1000 },
                {   1000, 1000, 1, 0, 0, 0, 0, 0, 0, -1, 1000, 1000 },
                {   1000, 1000, 1000, 1000, 1000, 1000, 1000, 1000, 1000, 1000, 1000, 1000 },
                {   1000, 1000, 1000, 1000, 1000, 1000, 1000, 1000, 1000, 1000, 1000, 1000 }
            };
        }
        if(ParentState != null)
        {
            for (int i = 0; i < this.board.GetLength(0); i++)
            {
                for (int j = 0; j < this.board.GetLength(1); j++)
                {
                    this.board[i, j] = ParentState.GetBoard()[i, j];
                }
            }
            this.ParentState = ParentState;
        }
        PopulatePlayerBoards(true);
        PopulatePlayerBoards(false);
        EvaulateCurrentState();
    }
    public int EvaulateCurrentState()
    {
        int p1Distance = FindTotalDistances(p1Pieces);
        int p2Distance = -FindTotalDistances(p2Pieces);
        int p1GoalPoints = CalcGoalPointScore(p1Pieces);
        int p2GoalPoints = -CalcGoalPointScore(p2Pieces);
        int distanceScore = -(p1Distance + p2Distance);
        int goalScore = p1GoalPoints + p2GoalPoints;
        value = distanceScore + goalScore;
        return value;
    }

    private int FindTotalDistances(int[,] pieces)
    {
        int distanceTotal = 0;
        int distance;
        for(int i = 0; i < board.GetLength(0); i++)
        {
            for (int j = 0; j < board.GetLength(1); j++)
            {
                if (pieces[i,j] != 0)
                {
                    distance = i - 2;
                    distanceTotal += distance;
                }
            }
        }
        return distanceTotal;
    }
    private int CalcGoalPointScore(int[,] pieces)
    {
        int score = 0;
        for(int i = 0;i < board.GetLength(0);i++)
        {
            for(int j = 0; j < board.GetLength(1); j++)
            {
                if (pieces[i,j] != 0)
                {
                    if(i == 2)
                    {
                        score++;
                    }
                }
            }
        }
        return score;
    }
    public int[,] GetBoard()
    {
        return board;
    }
    public void GeneratePossibleStates(int currentDepth, int maxDepth)
    {
        if(PossibleStates.Count > 0)
        {
            PossibleStates.Clear();
        }
        int depth = currentDepth;
        this.currentDepth = currentDepth;
        int pieceValue = -1;
        if(p1Turn == false)
        {
            pieceValue = 1;
        }
        else
        {
            pieceValue = -1;
        }
        if(currentDepth >= maxDepth)
        {
            Debug.Log("ended generation");
            return;
        }
        for(int i = 0;i < board.GetLength(0);i++)
        {
            for(int j = 0;j < board.GetLength(1);j++)
            {
                if (board[i,j] == pieceValue)
                {
                    GenerateState(0, i, j);
                    GenerateState(1, i, j);
                    GenerateState(2, i, j);
                    GenerateState(3, i, j);
                }
            }
        }
        depth++;
        if(PossibleStates.Count != 0)
        {
            foreach(GameState state in PossibleStates)
            {
                state.GeneratePossibleStates(depth, maxDepth);
            }
        }
    }
    public void GenerateState(int direction,int yPos, int xPos)
    {
        int newYpos = yPos;
        int newXpos = xPos;
        int moveSum = 0;
        int pieceValue = board[yPos,xPos];
        GameState possibleState = null;
        MoveDirections moveDirection = (MoveDirections)direction;
        switch (moveDirection)
        {
            case 0:
                newYpos = yPos - 1;
                moveSum = board[yPos,xPos] + board[newYpos, newXpos];
                break;
            case (MoveDirections)1:
                newYpos = yPos + 1;
                moveSum = board[yPos, xPos] + board[newYpos, newXpos];
                break;
            case (MoveDirections)2:
                newXpos = xPos - 1;
                moveSum = board[yPos, xPos] + board[newYpos, newXpos];
                break;
            case (MoveDirections)3:
                newXpos= xPos + 1;
                moveSum = board[yPos, xPos] + board[newYpos, newXpos];
                break;
        }
        if (moveSum == pieceValue)
        {
            possibleState = new GameState(!p1Turn, this);
            possibleState.GetBoard()[yPos, xPos] = 0;
            possibleState.GetBoard()[newYpos, newXpos] = pieceValue;
            if (newYpos != 9)
            {
                if ((possibleState.GetBoard()[newYpos, newXpos] + possibleState.GetBoard()[newYpos, newXpos + 1]) == 0)
                {
                    if ((possibleState.GetBoard()[newYpos + 1, newXpos + 1] + possibleState.GetBoard()[newYpos, newXpos + 1]) == -pieceValue)
                    {
                        possibleState.GetBoard()[newYpos, newXpos + 1] = 0;
                        possibleState.GetBoard()[newYpos + 1, newXpos + 1] = -pieceValue;
                    }

                }
                if ((possibleState.GetBoard()[newYpos, newXpos] + possibleState.GetBoard()[newYpos, newXpos - 1]) == 0)
                {
                    if ((possibleState.GetBoard()[newYpos + 1, newXpos - 1] + possibleState.GetBoard()[newYpos, newXpos - 1]) == -pieceValue)
                    {
                        possibleState.GetBoard()[newYpos, newXpos - 1] = 0;
                        possibleState.GetBoard()[newYpos + 1, newXpos - 1] = -pieceValue;
                    }
                }
            }
            possibleState.PopulatePlayerBoards(true);
            possibleState.PopulatePlayerBoards(false);
            PossibleStates.Add(possibleState);
            Debug.Log("new state added");
        }
        Debug.Log(currentDepth);
    }
    public void SetCurrentTurn(bool turn)
    {
        this.p1Turn = turn;
    }
    public bool GetCurrentTurn()
    {
        return this.p1Turn;
    }
    public void PopulatePlayerBoards(bool turn)
    {
        int[,] pawns = p1Pieces;
        int searchValue = 1;
        if(turn == false)
        {
            pawns = p2Pieces;
            searchValue = -1;
        }
        for (int i = 0; i < board.GetLength(0); i++) 
        {
            for(int j = 0; j < board.GetLength(1); j++)
            {
                if (board[i,j] == searchValue)
                {
                    pawns[i,j] = searchValue;
                }
                else
                {
                    pawns[i,j] = 0;
                }
            }
        }

    }
    public GameState GetParentState()
    {
        return ParentState;
    }
    public int[,] GetP1Pawns()
    {
        return p1Pieces;
    }
    public int[,] GetP2Pawns()
    {
        return p2Pieces;
    }
    public int GetEvalValue()
    {
        return value;
    }
    public void SetEval(int value)
    {
        this.value = value;
    }
    public void SetParentEval()
    {
        if(ParentState == null)
        {
            return;
        }
        if(PossibleStates.Count == 0)
        {
            ParentState.SetEval(this.EvaulateCurrentState());
        }
        ParentState.SetEval(this.value);
        Debug.Log("Set to " + this.value);
    }
    public void SetParentChainEval()
    {
        if (ParentState == null)
        {
            Debug.Log("no parent");
            return;
        }
        SetParentEval();
        ParentState.SetParentChainEval();
    }
    public GameState FindChildByValue(int value, bool turn)
    {
        GameState childState = null;
        if(PossibleStates.Count == 0)
        {
            return null;
        }
        foreach (GameState state in PossibleStates)
        {
            childState = state;
            if (state.GetEvalValue() == value && turn == p1Turn)
            {
                childState = state;
                return childState;
            }
            else
            {
                childState.FindChildByValue(value, turn);
            }
        }
        return childState;
    }
    public bool IsWinningState(bool turn)
    {
        bool winning = false;
        int[,] piecesToCheck;
        if(turn == true)
        {
            piecesToCheck = p1Pieces;
        }
        else
        {
            piecesToCheck = p2Pieces;
        }
        if(FindTotalDistances(piecesToCheck) == 0) 
        {
            winning = true;
        }
        return winning;
    }
    public List<GameState> GetPossibleStates()
    {
        return PossibleStates;
    }
    public void SetBoard(int[,] board)
    {
        for (int i = 0; i < board.GetLength(0); i++)
        {
            for(int j = 0; j < board.GetLength(1); j++)
            {
                this.board[i, j] = board[i, j];
            }
        }
    }
    public GameState FindMinMaxState(List<GameState> states, bool isMaximising)
    {
        int n = states.Count;
        for (int i = 0; i < n - 1; i++)
        {
            for (int j = 0; j < n - i - 1; j++)
            {
                if (states[j].EvaulateCurrentState() > states[j + 1].EvaulateCurrentState())
                {
                    GameState temp = states[j];
                    states[j] = states[j + 1];
                    states[j + 1] = temp;
                }
            }
        }
        GameState minMax;
        if(isMaximising == false)
        {
            minMax = states[0];
        }else
        {
            minMax= states[states.Count - 1];
        }
        return minMax;
    }
    public GameState FindLastStateParent(int depth, int maxDepth)
    {
        if(depth == 0 || PossibleStates.Count == 0)
        {
            return ParentState;
        }
        GameState lastParent = FindLastStateParent(depth -1 , maxDepth);
        return lastParent;
    }
}
