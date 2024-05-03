using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinScreenScript : MonoBehaviour
{
    public TMP_Text winnerText;

    private void Start()
    {
        if (GameManager.instance.blueScore > GameManager.instance.redScore)
        {
            winnerText.text = "Blue Wins\n turns:" + GameManager.instance.turn;
        }
        else
        {
            winnerText.text = "Red Wins\n turns:" + GameManager.instance.turn;
        }
    }
    public void RestartGame()
    {
        SceneManager.LoadScene(0);
    }
}
