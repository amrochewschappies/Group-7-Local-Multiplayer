using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public static GameManager gmInstance { get; private set; }

    private void Awake()
    {
        if (gmInstance != null && gmInstance != this)
        {
            Destroy(this);
        }
        else
        {
            gmInstance = this;
        }
        DontDestroyOnLoad(gameObject);
    }

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

    private void Start()
    {
        AudioManager.Instance.PlaySound("Announcer- Track1" , 1 , 0.3f);
    }

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
        if(hasDied) return;
        hasDied = true;
        if (player == player1)
        {
            StartCoroutine(showText());
            StartCoroutine(TextAnimation(player1text, Color.green, Color.blue));
        }else if (player == player2)
        {
            Debug.Log(player);
            StartCoroutine(showText());
            Debug.Log("Player 1 text working.");
            StartCoroutine(TextAnimation(player2text,Color.red, Color.magenta));
        }
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
    IEnumerator TextAnimation(TextMeshProUGUI textElement, Color color1, Color color2)
    {
        for (int i = 0; i < 10; i++)
        {
            textElement.color = color1;
            yield return new WaitForSeconds(0.5f);  
            textElement.color = color2;
            yield return new WaitForSeconds(0.5f);
        }
    }

    public IEnumerator waitBeforeLoading()
    {
        yield return new WaitForSeconds(2f);
        LoadEndScene();
    }

    private IEnumerator showText()
    {
        if (hasWon)
        {
            player1text.text = "You won!";
        }

        if (hasDied)
        {
            player1text.text = "You touched the magma, you lost!";
        }
   
        yield return new WaitForSeconds(0.5f);

        if (hasWon)
        {
            player2text.text = "You lost";       
        }

        if (hasDied)
        {
            player2text.text = "You win , player 1 touched the magma.";         
        }

    }
}
