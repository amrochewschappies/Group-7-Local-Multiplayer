using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameManger : MonoBehaviour
{
    //GM checks winner
    //GM checks loser
    //GM checks player Deaths
    //GM assits with pause sequence 
    //GM assists with audio queues
    [Header("References")]
    public TextMeshProUGUI player1text;
    public TextMeshProUGUI player2text;
    
    public GameObject player1;
    public GameObject player2;
    
    [Header("LiveGame Checks")]
    public bool hasWon = false;
    public bool hasDied = false;
    
    

    public void CheckWinner(GameObject player)
    {
        if (hasWon) return;
        
        hasWon = true;
        if (player == player1)
        {
            player1text.text = " You won!"; 
            player2text.text = " GAME OVER, you lost!"; 
            StartCoroutine(waitBeforeLoading());
        }
        else if (player == player2)
        {
            player1text.text = " GAME OVER, you lost! "; 
            player2text.text = " You won!";
            StartCoroutine(waitBeforeLoading());

        }
    }
    
    public void CheckDeath(GameObject player)
    {
        // when hitting lava        
        // show losetext then end scene
        // //make sure that the end scene is loaded here
    }
    
    public void LoadStartScene()
    {
        SceneManager.LoadScene("StartScene");
    }
    
    public void LoadMainScene()
    {
        SceneManager.LoadScene("GameScene");
    }
    
    public void LoadEndScene()
    {
        SceneManager.LoadScene("EndScene");    
    }

    public void OnApplicationQuit()
    {
        Application.Quit();
    }

    IEnumerator waitBeforeLoading()
    {
        yield return new WaitForSeconds(2f);
        LoadEndScene();
    }
}
