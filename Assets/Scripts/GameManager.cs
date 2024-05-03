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

    public GameObject modeSelectPanel;

    public TMP_Text currentTurnText;

    public int turn = 0;
    public bool isP1Turn = true;
    public int blueScore = 0;
    public int redScore = 0;

    public MovingPoint currentlySelectedPawn;
    public MovingPoint previouslySelectedPawn;

    public static GameManager instance;
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
    }
    void Start()
    {
        DisplayCurrentTurn();
    }

    // Update is called once per frame
    void Update()
    {
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
        modeSelectPanel.SetActive(false);
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
        turn++;
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
}
