using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public GameObject pointPrefab;
    BoardPoint[,] points = new BoardPoint[8,8];
    GameObject[] player1Pieces = new GameObject[4];
    GameObject[] player2Pieces = new GameObject[4];
    public Vector3 startPoint;
    public Vector3 placePoint;
    public float lengthWidth;
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
                pointName = i + "_" + j;
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
}
