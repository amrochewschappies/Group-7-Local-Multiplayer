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
    public Camera PodiumCamera;
    public GameObject Podium;
    public GameObject Canvas;
    
    [Header("LiveGame Checks")]
    public bool hasWon = false;
    public bool hasDied = false;
    [SerializeField]private float timer = 0f; 
    [SerializeField]private int currentTime = 0;


    
    private void Start()
    {
        currentTime = 0;
        Canvas.SetActive(false);
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= 1f)
        {
            IncrementTimer(1);
            timer = 0f; 
        }

        if (currentTime >= 8.5f && !hasWon && !hasDied)
        {
            ActivatePlayersMovement();    
            Canvas.SetActive(true);
        }
    }
    
    void IncrementTimer(int incrementAmount)
    {
        currentTime += incrementAmount;
    }
    
    //Timed Event functions
    void ActivatePlayersMovement()
    {
        _player1.enabled = true;
        _player2.enabled = true;
    }
    
    void DeactivatePlayersMovement()
    {
        _player1.enabled = false;
        _player2.enabled = false;
    }
    
    //win conditions
    public void CheckWinner(GameObject player)
    {
        if (hasWon) return;
        hasWon = true;
        Canvas.SetActive(false);
        PodiumCamera.enabled = true;
        if (player == player1)
        {
            
            player1.transform.position = new Vector3(Podium.transform.position.x, Podium.transform.position.y + 2, Podium.transform.position.z);
            player2.transform.position = new Vector3(Podium.transform.position.x - 1f, Podium.transform.position.y + 2, Podium.transform.position.z);
            player1.transform.rotation = Quaternion.Euler(0f, 360f, 0f);
            player2.transform.rotation = Quaternion.Euler(0f, 360f, 0f);
            StartCoroutine(waitBeforeLoading());
        }
        else if (player == player2)
        {
            
            player2.transform.position = new Vector3(Podium.transform.position.x, Podium.transform.position.y + 2, Podium.transform.position.z);
            player1.transform.position = new Vector3(Podium.transform.position.x - 1f, Podium.transform.position.y + 2, Podium.transform.position.z);
            player1.transform.rotation = Quaternion.Euler(0f, 360f, 0f);
            player2.transform.rotation = Quaternion.Euler(0f, 360f, 0f);
            StartCoroutine(waitBeforeLoading());

        }
    }
    
    public void CheckDeath(GameObject player)
    {
        if (hasDied) return;
        hasDied = true;
        Canvas.SetActive(false);
        PodiumCamera.enabled = true;
        if (player == player1)
        {
            
            player2.transform.position = new Vector3(Podium.transform.position.x, Podium.transform.position.y + 2, Podium.transform.position.z);
            player1.transform.position = new Vector3(Podium.transform.position.x - 1f, Podium.transform.position.y + 2, Podium.transform.position.z);
            player1.transform.rotation = Quaternion.Euler(0f, 360f, 0f);
            player2.transform.rotation = Quaternion.Euler(0f, 360f, 0f);


        }
        else if (player == player2)
        {
            player1.transform.position = new Vector3(Podium.transform.position.x, Podium.transform.position.y + 2, Podium.transform.position.z);
            player2.transform.position = new Vector3(Podium.transform.position.x - 1f, Podium.transform.position.y + 2, Podium.transform.position.z);
            player1.transform.rotation = Quaternion.Euler(0f, 360f, 0f);
            player2.transform.rotation = Quaternion.Euler(0f, 360f, 0f);
        }
    }
    
    
    //scenes
    public void LoadStartScene()
    {
        SceneManager.LoadScene("StartScene");
    }
    
    public void LoadMainScene()
    {
        DeactivatePlayersMovement();
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
