﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionToMain : MonoBehaviour
{
    LevelManager levelManager;
    public int mainMenuSceneIndex = 4;
    // Use this for initialization
    void Start()
    {
        levelManager = GameObject.FindObjectOfType<LevelManager>();

    }
    void LoadMainMenu()
    {
        levelManager.LoadLevel(mainMenuSceneIndex);

    }
}