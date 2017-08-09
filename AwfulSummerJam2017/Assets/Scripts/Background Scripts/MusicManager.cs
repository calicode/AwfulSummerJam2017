﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{

    static MusicManager instance = null;

    public AudioClip[] musicArray;
    AudioSource audioSource;
    const string PLAYER_PREFS_VOLUME = "AWFULJAM_2017_BOOZE-BLUES_VOLUMEKEY";



    // Use this for initialization
    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            Debug.Log("Destroying extra music manager");
        }


        else
        {
            instance = this; 
            DontDestroyOnLoad(gameObject);
        }


        audioSource = GetComponent<AudioSource>();
        audioSource.volume = GetVolume();
        PlayMusicTrack(SceneManager.GetActiveScene().buildIndex); // only needed for testing so main menu has a song. can remove in final build

    }

    public void OnLevelWasLoaded()
    {
        PlayMusicTrack(SceneManager.GetActiveScene().buildIndex);

    }

    public void PlayMusicTrack(int sceneIndex)
    {
        AudioClip clip = musicArray[sceneIndex];

        if (clip)
        {
            if (audioSource.isPlaying && clip.name == audioSource.clip.name) 
            {
                return; // if its the same song don't start playing it again. This is so title screen music transitions to first level without interruption. 
            } 

            audioSource.clip = clip;
            audioSource.PlayDelayed(1f);
            audioSource.loop = true;

        }
        else
        {
            Debug.LogWarning("No clip in MusicManager's musicArray for this scene. Add clip or just ignore this"); 
        }

    }

    // set by SetMusicVolume script which is on main menu canvas. 

    public void SetVolume(float newVolume)
    {

        if (newVolume <= 1.0f)
        {
            PlayerPrefs.SetFloat(PLAYER_PREFS_VOLUME, newVolume);
            audioSource.volume = GetVolume();
        }
        else 
        { 
            Debug.LogWarning("Tried to set volume too high, this shouldn't appear"); 
        }

    }

    public float GetVolume()
    {
        float prefsVolume = PlayerPrefs.GetFloat(PLAYER_PREFS_VOLUME);


        if (prefsVolume <= 1.0f && prefsVolume >= 0f) 
        {
            return prefsVolume;
        }
        else
        {
            Debug.LogWarning("Playerprefs volume somehow got set too high or is too low, sending back default volume.");
            SetVolume(0f);
            return 0;
        }
    }
}
