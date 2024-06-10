using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardPoint : MonoBehaviour
{
    public GameObject occupyingPiece;
    public MovingPoint movingPoint;
    public bool isFree = true;
    public bool isScorePoint = false;
    public BoardPoint[] adjacentPoints = new BoardPoint[4];

    private void Start()
    {
        isFree = true;
        StartCoroutine(DelayCheck());
    }
    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("P1Piece") || other.CompareTag("P2Piece"))
        {
            occupyingPiece = other.gameObject;
            movingPoint = occupyingPiece.GetComponent<MovingPoint>();
            isFree = false;
            //Debug.Log("contact" + gameObject.name);
            for (int i = 0; i < adjacentPoints.Length; i++)
            {
                if (adjacentPoints[i] != null)
                {
                    //Debug.Log("check");
                    movingPoint.MovableLocations[i] = adjacentPoints[i];
                }
            }
            if (isScorePoint)
            {
                AddWinPoint(other.tag, 1);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        //Debug.Log("ExitContact");
        occupyingPiece = null;
        movingPoint = null;
        isFree = true;
        if (isScorePoint)
        {
            AddWinPoint(other.tag, -1);
        }
    }
    private void Update()
    {
        //CheckAdjacent(); 
    }

    public void CheckAdjacent()
    {
        if (Physics.Raycast(transform.position, Vector3.forward, out RaycastHit hit, 1000, 1, QueryTriggerInteraction.Collide))
        {
            //Debug.DrawRay(transform.position, hit.point, Color.red);
            adjacentPoints[0] = hit.transform.GetComponent<BoardPoint>();
        }
        if (Physics.Raycast(transform.position, Vector3.left, out RaycastHit hit1, 1000, 1, QueryTriggerInteraction.Collide))
        {
            //Debug.DrawRay(transform.position, hit.point, Color.yellow);
            adjacentPoints[1] = hit1.transform.GetComponent<BoardPoint>();
        }
        if (Physics.Raycast(transform.position, -Vector3.forward, out RaycastHit hit2, 1000, 1, QueryTriggerInteraction.Collide))
        {
            //Debug.DrawRay(transform.position, hit.point, Color.blue);
            adjacentPoints[3] = hit2.transform.GetComponent<BoardPoint>();
        }
        if (Physics.Raycast(transform.position, -Vector3.left, out RaycastHit hit3, 1000, 1, QueryTriggerInteraction.Collide))
        {
            //Debug.DrawRay(transform.position, hit.point, Color.green);
            adjacentPoints[2] = hit3.transform.GetComponent<BoardPoint>();
        }
    }
    IEnumerator DelayCheck()
    {
        yield return new WaitForSeconds(0.5f);

        CheckAdjacent();
    }
    public void AddWinPoint(string tag, int scorer)
    {
        if (tag == "P1Piece")
        {
            GameManager.instance.blueScore += scorer;
        }
        else
        {
            GameManager.instance.redScore += scorer;
        }
    }
}

