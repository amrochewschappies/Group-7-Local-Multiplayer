using System;
using System.Collections;
using UnityEngine;

public class CoinTrigger : MonoBehaviour
{
    private GameManager _gameManger;

    private void Start()
    {
        _gameManger = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
     
        if (other.CompareTag("Player"))
        {
            GameObject hitInfo = other.gameObject;
            _gameManger.CheckWinner(hitInfo);
            _gameManger.waitBeforeLoading();
            StartCoroutine(deleteCrown());
        }
    }

    IEnumerator deleteCrown()
    {
        yield return new WaitForSeconds(0.01f);
        Destroy(gameObject);
    }
}
