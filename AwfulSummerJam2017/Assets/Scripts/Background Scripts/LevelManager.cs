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
        DontDestroyOnLoad(gameObject); //Makes sure the LevelManager doesn't go anywhere once it's loaded
    }

    //Both these functions loads the level. Overloaded function to make sure we can use indexes or strings
    public void LoadLevel(string levelName)
    {
        SceneManager.LoadSceneAsync(levelName);

    }
    public void LoadLevel(int sceneIndex)
    {
        SceneManager.LoadSceneAsync(sceneIndex);

    }





}
