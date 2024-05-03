using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPoint : MonoBehaviour
{
    public bool isSelected = false;
    public bool isP1Piece = true;
    public BoardPoint[] MovableLocations = new BoardPoint[4];
    public GameObject[] movablePoints = new GameObject[4];
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void MoveInDirection(int direction)
    {
        MovePawn(direction);
        StartCoroutine(DropOpponent());
        GameManager.instance.EndTurn();
    }
    public void MovePawn(int direction)
    {
        transform.position = MovableLocations[direction].transform.position;
    }
    public void ShowMovePoints()
    {
        if (isSelected)
        {
            for (int i = 0; i < MovableLocations.Length; i++)
            {
                if (MovableLocations[i] != null && MovableLocations[i].GetComponent<BoardPoint>().isFree)
                {
                    movablePoints[i].SetActive(true);
                    movablePoints[i].transform.position = MovableLocations[i].transform.position;
                }
            }
        }
        else
        {
            for (int i = 0; i < MovableLocations.Length; i++)
            {
                movablePoints[i].SetActive(false);
            }
        }
    }
    IEnumerator DropOpponent()
    {
        yield return new WaitForSeconds(0.1f);
        for (int i = 0; i < MovableLocations.Length; i++)
        {
            if (MovableLocations[i] != null && MovableLocations[i].isFree == false)
            {
                if (i > 0 && i < MovableLocations.Length - 1)
                {
                    if (MovableLocations[i].movingPoint.isP1Piece != isP1Piece && MovableLocations[i].movingPoint.MovableLocations[MovableLocations.Length - 1].isFree == true)
                    {
                        MovableLocations[i].movingPoint.MovePawn(3);
                    }
                }

            }
        }
    }
}
