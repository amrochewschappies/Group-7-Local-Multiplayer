using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneManage : MonoBehaviour
{
    public static SceneManage smInstance { get; private set; }

    private void Awake()
    {
        // Singleton pattern
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
   // public GameObject LoadingAnim;

    private void Start()
    {
        currentTime = 0;
        timer = 0;

        // Ensure LoadingAnim is deactivated at the start
        //LoadingAnim.SetActive(false);

        // Subscribe to scene loaded event
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Update()
    {
        if (!isLoaded)
        {
            timer += Time.deltaTime;
            if (timer >= 1f)
            {
                currentTime += 1;
                timer = 0f;
            }

            if (currentTime >= 42f)
            {
                LoadStartScene();
                isLoaded = true;
            }
        }
    }

    // Scenes
    public void LoadStartScene()
    {
        SceneManager.LoadScene("StartScene");
    }

    public void LoadMainScene()
    {
        /*if (LoadingAnim != null)
        {
            LoadingAnim.SetActive(true);
        }*/
        StartCoroutine(WaitBeforeLoadingMain());
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
        LoadStartScene();
    }

    private IEnumerator WaitBeforeLoadingMain()
    {
        Debug.Log("Main scene is loading...");
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene("GameScene");
        Debug.Log("Game scene loaded.");
    }
    
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        /*if (scene.name == "StartScene")
        {
            if (LoadingAnim != null)
            {
                LoadingAnim.SetActive(false);
            }
        }
        else
        {
            if (LoadingAnim != null)
            {
                LoadingAnim.SetActive(false);
            }
        }*/
    }

    private void OnDestroy()
    {
        // Unsubscribe from the sceneLoaded event when the object is destroyed
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
