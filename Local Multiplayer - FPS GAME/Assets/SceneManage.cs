using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneManage : MonoBehaviour
{
    public static SceneManage smInstance { get; private set; }

    private void Awake()
    {
        if (smInstance != null && smInstance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            smInstance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    [SerializeField] private float timer = 0f;
    [SerializeField] private int currentTime = 0;
    [SerializeField] private bool isLoaded = false;
    private bool isLoading = false; 

    private void Start()
    {
        currentTime = 0;
        timer = 0;
        
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "LoadingScene" && !isLoaded)
        {
            timer += Time.deltaTime;
            if (timer >= 1f)
            {
                currentTime += 1;
                timer = 0f;
            }

            if (currentTime >= 42f)
            {
                LoadTutorialScene();
                isLoaded = true;
            }
        }
        else if (isLoaded)
        {
            currentTime = 0;
            timer = 0;
        }
    }

    public void LoadStartScene()
    {
        isLoaded = false;
        SceneManager.LoadScene("StartScene");
        Debug.Log("StartScene is loading");
    } 

    public void LoadTutorialScene()
    {
        isLoaded = false;
        SceneManager.LoadScene("TutorialScene");
    }
    
    public void LoadMainScene()
    {
        if (isLoaded || isLoading) 
        {
            Debug.Log("Main Scene is already loading or has been loaded.");
            return;
        }

        isLoading = true; 
        StartCoroutine(WaitBeforeLoadingMain());
    }

    public void OnApplicationQuit()
    {
        Application.Quit();
    }
    
    public IEnumerator waitBeforeLoading()
    {
        yield return new WaitForSeconds(15f);
        LoadStartScene();
        GameManager.gmInstance.isSceneLoading = false;
    }
    
    private IEnumerator WaitBeforeLoadingMain()
    {
        Debug.Log("Main scene is loading...");
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene("GameScene");
        isLoaded = true;
        isLoading = false; 
        Debug.Log("Game scene loaded.");
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {

        if (scene.name == "StartScene")
        {
            isLoaded = false;  
            Debug.Log("Back to StartScene.");
        }
        else if (scene.name == "GameScene")
        {
            isLoaded = true;  
            Debug.Log("Loaded GameScene.");
        }
        else if (scene.name == "TutorialScene")
        {
            isLoaded = true;
            Debug.Log("Loaded TutorialScene.");
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
