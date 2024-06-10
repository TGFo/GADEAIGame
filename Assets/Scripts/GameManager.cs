using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject[] player1Pieces;
    public GameObject[] player2Pieces;
    public BoardManager boardManager;
    public GameObject player1Prefab;
    public GameObject player2Prefab;

    public AIController AIPlayer;

    public GameObject modeSelectPanel;
    public GameObject playerSelectPanel;
    public GameObject difficultySelectPanel;

    public TMP_Text currentTurnText;

    public bool AIPlayerActive = false;
    public bool AIPlayerTurn = false;
    public int turn = 0;
    public bool isP1Turn = true;
    public int blueScore = 0;
    public int redScore = 0;

    public MovingPoint currentlySelectedPawn;
    public MovingPoint previouslySelectedPawn;

    public static GameManager instance;

    public GameState gameState;
    public GameState nextState;
    public GameState futureState;

    public TMP_Text debugText;

    public int currentEval;
    int index = 0;

    // Start is called before the first frame update
    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(this);
        }else
        {
            instance = this;
        }
        gameState = new GameState(isP1Turn);
    }
    void Start()
    {
        DisplayCurrentTurn();
    }

    // Update is called once per frame
    void Update()
    {
        currentEval = gameState.GetEvalValue();
        if(Input.GetKeyUp(KeyCode.G))
        {
            string boardString = "";
            for (int i = 0; i < gameState.GetBoard().GetLength(0); i++)
            {
                for(int j = 0; j < gameState.GetBoard().GetLength(1); j++)
                {
                    boardString += gameState.GetBoard()[i, j] + " ";
                }
                boardString += "\n";
            }
            debugText.text = boardString;
            index++;
        }
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Input.GetMouseButtonDown(0))
        {
            if(Physics.Raycast(ray, out RaycastHit hit, 10000, 1, QueryTriggerInteraction.Ignore))
            {
                if(hit.transform.TryGetComponent<MovingPoint>(out MovingPoint point))
                {
                    Debug.Log(hit.transform.name);
                    HighlightPossibleMoves(point);
                }
            }
        }
        if(blueScore == 4 || redScore == 4)
        {
            SceneManager.LoadScene(1);
        }
    }
    public void PlayerVsPlayerBtn()
    {
        player1Pieces = boardManager.PlacePlayer1Pieces(player1Prefab);
        player2Pieces = boardManager.PlacePlayer2Pieces(player2Prefab);
        boardManager.GenerateArrayBoard();
        modeSelectPanel.SetActive(false);
    }
    public void PlayerVsAIBtn()
    {
        AIPlayerActive = true;
        playerSelectPanel.SetActive(true);
        modeSelectPanel.SetActive(false);
    }
    public void SelectPlayerPos(bool isFirstPlayer)
    {
        AIPlayerTurn = !isFirstPlayer;
        player1Pieces = boardManager.PlacePlayer1Pieces(player1Prefab);
        player2Pieces = boardManager.PlacePlayer2Pieces(player2Prefab);
        boardManager.GenerateArrayBoard();
        gameState.SetCurrentTurn(!AIPlayerTurn);
        difficultySelectPanel.SetActive(true);
        playerSelectPanel.SetActive(false);
    }
    public void DifficultySelectBtn(int difficulty)
    {
        AIPlayer.farsightVal = difficulty;
        PerformAITurn();
        difficultySelectPanel.SetActive(false);
    }
    public void SelectPawn(MovingPoint point)
    {
        if(previouslySelectedPawn == null)
        {
            currentlySelectedPawn = point;
            previouslySelectedPawn = point;
            currentlySelectedPawn.isSelected = true;
            showSelectedMoves();
            return;
        }
        previouslySelectedPawn = currentlySelectedPawn;
        currentlySelectedPawn = point;
        previouslySelectedPawn.isSelected = false;
        currentlySelectedPawn.isSelected = true;
        showSelectedMoves();
    }
    public void DeselectPawn()
    {
        if(previouslySelectedPawn != null)
        {
            previouslySelectedPawn.isSelected = false;
            currentlySelectedPawn.isSelected = false;
            showSelectedMoves();
        }
        currentlySelectedPawn = null;
        previouslySelectedPawn = null;
    }
    public void HighlightPossibleMoves(MovingPoint point)
    {
        if(AIPlayerActive == true && isP1Turn == AIPlayerTurn)
        {
            Debug.Log("no selection");
            return;
        }
        if(point.isP1Piece == isP1Turn)
        {
            SelectPawn(point);
        }else
        {
            DeselectPawn();
        }
    }
    public void showSelectedMoves()
    {
        currentlySelectedPawn.ShowMovePoints();
        previouslySelectedPawn.ShowMovePoints();
    }
    public void EndTurn()
    {
        isP1Turn = !isP1Turn;
        DeselectPawn();
        DisplayCurrentTurn();
        //Debug.Log("End turn");
        turn++;
        StartCoroutine(DelayCheck());
        
        
    }
    public void DisplayCurrentTurn()
    {
        if (isP1Turn)
        {
            currentTurnText.text = "Blue turn";
        }
        else
        {
            currentTurnText.text = "Red turn";
        }
    }
    public void PerformAITurn()
    {
        if(AIPlayerActive == false)
        {
            nextState = gameState;
            return;
        }
        if(AIPlayerTurn == isP1Turn)
        {
            gameState.GeneratePossibleStates(0, AIPlayer.farsightVal);
            nextState = AIPlayer.MinMax(gameState, AIPlayer.farsightVal, AIPlayerTurn, AIPlayer.farsightVal);
            Debug.Log(nextState.GetCurrentTurn() + " " + gameState.GetCurrentTurn());
            boardManager.GenerateBoardFromArray(nextState.GetBoard());
            EndTurn();
        }
    }
    IEnumerator DelayCheck()
    {
        yield return new WaitForSeconds(0.2f);
        Debug.Log("first wait done");


        boardManager.GenerateArrayBoard();

        yield return new WaitForSeconds(0.01f);
        Debug.Log("second wait done");
        gameState.SetBoard(boardManager.gameBoard);
        yield return new WaitForSeconds(0.01f);
        Debug.Log("third wait done");
        boardManager.GenerateBoardFromArray(gameState.GetBoard());
        PerformAITurn();

    }
}
