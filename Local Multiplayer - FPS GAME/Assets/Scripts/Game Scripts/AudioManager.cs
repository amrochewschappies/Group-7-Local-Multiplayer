using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }
    [SerializeField] private AudioSource player1Source;
    [SerializeField] private AudioSource player2Source;

    [SerializeField] private Dictionary<string, AudioClip> soundLibrary = new Dictionary<string, AudioClip>();

    private void Awake()
    {
        // Singleton pattern
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
      
        AudioClip[] clips = Resources.LoadAll<AudioClip>("Audio");
        
        foreach (AudioClip clip in clips)
        {
            soundLibrary[clip.name] = clip;
            //Debug.Log($"Loaded audio clip: {clip.name}");
        }
        
        if (player1Source == null)
        {
            //Debug.LogError("Player 1 AudioSource is not assigned!");
        }
        if (player2Source == null)
        {
           // Debug.LogError("Player 2 AudioSource is not assigned!");
        }
        
    }

    public void PlaySound(string clipName, int playerNumber, float volume)
    {
        if (soundLibrary.TryGetValue(clipName, out AudioClip clip))
        {
            AudioSource targetSource = playerNumber == 1 ? player1Source : player2Source;

            if (targetSource != null)
            {
                targetSource.clip = clip;
                targetSource.volume = volume;
                targetSource.Play();
            }
            else
            {
               // Debug.LogError($"AudioSource for Player {playerNumber} is not assigned!");
            }
        }
        else
        {
           // Debug.LogWarning($"AudioClip with name {clipName} not found in sound library.");
        }
    }


    public void StopSound(int playerNumber)
    {
        AudioSource targetSource = playerNumber == 1 ? player1Source : player2Source;

        if (targetSource != null)
        {
            targetSource.Stop();
        }
        else
        {
            // Debug.LogError($"AudioSource for Player {playerNumber} is not assigned!");
        }
    }
    

}