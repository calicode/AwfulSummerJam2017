using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{

    static string currentLevelName; // will be used later to reload current level


    // turn into singleton

    void Awake()
    {

        DontDestroyOnLoad(gameObject);
    }

    public void LoadLevel(string levelName)
    {

        SceneManager.LoadSceneAsync(levelName);

    }
    public void LoadLevel(int sceneIndex)
    {
        SceneManager.LoadSceneAsync(sceneIndex);

    }





}
