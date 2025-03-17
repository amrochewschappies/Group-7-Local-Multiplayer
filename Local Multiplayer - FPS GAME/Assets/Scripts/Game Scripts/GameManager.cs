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
    public PlayerController _player1;
    public PlayerController _player2;
    
    
    public GameObject player1;
    public GameObject player2;
    
    [Header("LiveGame Checks")]
    public bool hasWon = false;
    public bool hasDied = false;
    [SerializeField]private float timer = 0f; 
    [SerializeField]private int currentTime = 0; 
    
    
    private void Start()
    {
        currentTime = 0;
        _player1.enabled = false;
        _player2.enabled = false;
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= 1f)
        {
            IncrementTimer(1);
            timer = 0f; 
        }
    }

    void IncrementTimer(int incrementAmount)
    {
        currentTime += incrementAmount;
    }
    
    //Event functions
    
    //win conditions
    public void CheckWinner(GameObject player)
    {
        if (hasWon) return;
        
        hasWon = true;
        if (player == player1)
        {   
            //make camera move to player 1
            StartCoroutine(waitBeforeLoading());
        }
        else if (player == player2)
        {

            StartCoroutine(waitBeforeLoading());

        }
    }
    
    public void CheckDeath(GameObject player)
    {
        if(hasDied) return;
        hasDied = true;
        if (player == player1)
        {
            

        }else if (player == player2)
        {


        }
    }
    
    
    //scenes
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


    public IEnumerator waitBeforeLoading()
    {
        yield return new WaitForSeconds(15f);
        LoadEndScene();
    }
    
}
