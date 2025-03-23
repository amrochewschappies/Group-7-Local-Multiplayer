using System;
using UnityEngine;

public class LavaTrigger : MonoBehaviour
{
    private GameManager _gameManger;

    private void Awake()
    {
        _gameManger = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameObject hitPlayer = other.gameObject;
            
            if (_gameManger != null)
            {
                _gameManger.CheckDeath(hitPlayer);
                SceneManage.smInstance.waitBeforeLoading();
                Debug.Log("Loading Back to start Scene");
            }
            else
            {
                Debug.LogError("GameManager is not assigned properly.");
            }
        }
    }
    
    
}
