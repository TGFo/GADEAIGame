using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public GameObject pointPrefab;
    public BoardPoint[,] points = new BoardPoint[8, 8];
    GameObject[] player1Pieces = new GameObject[4];
    GameObject[] player2Pieces = new GameObject[4];
    public Vector3 startPoint;
    public Vector3 placePoint;
    public float lengthWidth;
    public int[,] gameBoard = new int[12, 12];
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void Awake()
    {
        GenerateBoard();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public void GenerateBoard()
    {
        string pointName;
        GameObject pointObject;
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                pointObject = Instantiate(pointPrefab);
                points[i, j] = pointObject.GetComponent<BoardPoint>();
                pointName = i + "," + j;
                pointObject.name = pointName;
                pointObject.transform.position = placePoint;
                placePoint.x = placePoint.x + (lengthWidth / 8);
                if(i == 0)
                {
                    points[i, j].isScorePoint = true;
                }
                else
                {
                    points[i, j].isScorePoint = false;
                }
            }
            placePoint.x = startPoint.x;
            placePoint.z = placePoint.z - (lengthWidth / 8);
        }
    }
    public GameObject[] PlacePlayer1Pieces(GameObject p1Piece)
    {
        for(int i = 0; i < 4; i++) 
        {
            player1Pieces[i] = Instantiate(p1Piece);
            player1Pieces[i].transform.position = points[7 - i, 0].transform.position;
        }
        return player1Pieces;
    }
    public GameObject[] PlacePlayer2Pieces(GameObject p2Piece)
    {
        for (int i = 0; i < 4; i++)
        {
            player2Pieces[i] = Instantiate(p2Piece);
            player2Pieces[i].transform.position = points[7 - i, 7].transform.position;
        }
        return player2Pieces;
    }
    public void GenerateArrayBoard()
    {
        for(int i = 0; i < gameBoard.GetLength(0); i++)
        {
            for(int j = 0; j < gameBoard.GetLength(1); j++)
            {
                if(i <= 1 || i >= 10)
                {
                    gameBoard[i, j] = 1000;
                }
                if(j <= 1 || j >= 10)
                {
                    gameBoard[i, j] = 1000;
                }
            }
        }
        for(int i = 2; i < gameBoard.GetLength(0) - 2; i++)
        {
            for(int j = 2; j < gameBoard.GetLength(1) - 2; j++)
            {
                if (points[i-2, j-2].isFree == true)
                {
                    gameBoard[i, j] = 0;
                }
                else
                {
                    if(points[i - 2, j - 2].occupyingPiece.gameObject.tag == "P1Piece")
                    {
                        gameBoard[i, j] = 1;
                    }
                    if(points[i - 2, j - 2].occupyingPiece.gameObject.tag == "P2Piece")
                    {
                        gameBoard[i, j] = -1;
                    }
                }
            }
        }
    }
    public void GenerateBoardFromArray(int[,] gameBoard)
    {
        int P1Counter = 0;
        int P2Counter = 0;
        for (int i = 2; i < gameBoard.GetLength(0) - 2; i++)
        {
            for (int j = 2; j < gameBoard.GetLength(1) - 2; j++)
            {
                if ((gameBoard[i, j] != 1000))
                {
                    if (gameBoard[i, j] == 0)
                    {
                        points[i - 2, j - 2].isFree = true;
                        points[i - 2, j - 2].occupyingPiece = null;
                    }
                    if (P1Counter < 4)
                    {
                        if (gameBoard[i, j] == 1)
                        {
                            player1Pieces[P1Counter].transform.position = points[i - 2, j - 2].transform.position;
                            points[i - 2, j - 2].isFree = false;
                            points[i - 2, j - 2].occupyingPiece = player1Pieces[P1Counter];
                            P1Counter++;
                        }
                    }
                    if (P2Counter < 4)
                    {
                        if (gameBoard[i , j] == -1)
                        {
                            player2Pieces[P2Counter].transform.position = points[i - 2, j - 2].transform.position;
                            points[i - 2, j - 2].isFree = false;
                            points[i - 2, j - 2].occupyingPiece = player2Pieces[P2Counter];
                            P2Counter++;
                        }
                    }
                }
            }
        }
        GenerateArrayBoard();
    }
}
