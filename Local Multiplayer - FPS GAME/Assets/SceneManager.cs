using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneManage : MonoBehaviour
{
    public static SceneManage  smInstance { get; private set; }

    private void Awake()
    {
        // Singleton pattern
        if (smInstance!= null && smInstance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            smInstance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    [SerializeField]private float timer = 0f; 
    [SerializeField]private int currentTime = 0;
    [SerializeField]private bool  isLoaded ;

    private void Update()
    {
        if (!isLoaded)
        {
            timer += Time.deltaTime;
            if (timer >= 1f)
            {
                IncrementTimer(1);
                timer = 0f;
            }

            if (currentTime >= 43f)
            {
                LoadStartScene();
            }
            else
            {
                isLoaded = true;
            }
        }
    }


    public void IncrementTimer(int incrementAmount)
    {
        currentTime += incrementAmount;
    }

    //scenes
    public void LoadStartScene()
    {
        SceneManager.LoadScene("StartScene");
    }
    
    public void LoadMainScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("GameScene");
    }
    
    public void LoadEndScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("EndScene");    
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
    
}
