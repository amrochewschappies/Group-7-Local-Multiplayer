using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManger : MonoBehaviour
{
    //GM checks winner
    //GM checks lose
    //GM checks Death
    //GM assits with pause sequence 
    //GM assists with audio queue
    //GM
    public bool hasWon = false;
    public TextMeshProUGUI player1text;
    public TextMeshProUGUI player2text;
    
    public GameObject player1;
    public GameObject player2;


    public void CheckWinner(GameObject player)
    {
        if (hasWon) return;
        
        hasWon = true;
        if (player == player1)
        {
            player1text.text = " You won!"; 
            player2text.text = " GAME OVER, you lost!"; 
        }
        else if (player == player2)
        {
            player1text.text = " GAME OVER, you lost! "; 
            player2text.text = " You won!";         
        }
    }
    
}
