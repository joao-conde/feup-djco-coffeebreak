﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public static GameManager instance = null;
    public int initialPlayerCoins;
    public float musicMultiplier = 1;
    public float sfxMultiplier = 1;

    public void setMusicMultiplier (float mult) {
        musicMultiplier = mult;
    }

    public void setSFXMultiplier (float mult) {
        sfxMultiplier = mult;
    }

    private void Awake () {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy (gameObject);

        DontDestroyOnLoad (gameObject);
    }

    public void EndGame () {
        LoadScene (3);
    }

    public void WinGame () {
        LoadScene (4);
    }

    private void LoadScene (int sceneIndex) {
        SceneManager.LoadScene (sceneIndex);
    }
}