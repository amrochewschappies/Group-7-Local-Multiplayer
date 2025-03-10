using System;
using System.Collections;
using UnityEngine;

public class CoinTrigger : MonoBehaviour
{
    private GameManger _gameManger;

    private void Start()
    {
        _gameManger = GameObject.Find("======GameManager=====").GetComponent<GameManger>();
    }

    private void OnTriggerEnter(Collider other)
    {
     
        if (other.CompareTag("Player"))
        {
            GameObject hitInfo = other.gameObject;
            _gameManger.CheckWinner(hitInfo);
            StartCoroutine(deleteCrown());
        }
    }

    IEnumerator deleteCrown()
    {
        yield return new WaitForSeconds(0.1f);
        Destroy(gameObject);
    }
}
