using System;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    //audio manager is a singleton
    public static AudioManager instance { get; private set; }
    
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(gameObject);
    }
    // we need an audio source to hear the audio from = audiosource
    // we need a place to store all audios 
    // audios need to be accessible by index & assigned to the audio source
    // volume of all the clips


    private AudioSource player1Source;
    private AudioSource player2Source;
    private Dictionary<string, AudioClip> soundLibrary = new Dictionary<string, AudioClip>();
    private void Start()
    {
        player1Source = GameObject.Find("player1Camera").GetComponent<AudioSource>();
        player2Source = GameObject.Find("player2Camera").GetComponent<AudioSource>();
        foreach (AudioClip clip in Resources.LoadAll<AudioClip>("Audio"))
        {
            soundLibrary[clip.name] = clip;
        }
    }

    public void PlaySound(AudioClip clip, float vol, float pitch )
    {
        //    
    }

    public void StopSound()
    {
        //  
    }
}
