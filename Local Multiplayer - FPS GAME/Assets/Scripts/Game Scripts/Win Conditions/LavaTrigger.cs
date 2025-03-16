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
            _gameManger.CheckDeath(hitPlayer);
            _gameManger.waitBeforeLoading();
        }
    }
    
    
}
