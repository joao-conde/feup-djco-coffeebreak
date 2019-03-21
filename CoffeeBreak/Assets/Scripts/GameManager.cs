using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    public static GameManager instance = null;
    public int initialPlayerCoins = 0;

    bool gameOver = false;

    private void Awake () {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy (gameObject);

        DontDestroyOnLoad (gameObject);
    }

    public void EndGame () {
        if (!gameOver) {
            gameOver = true;
            //Invoke ("Restart", 2f);
            LoadScene (2);
        }
    }

    private void LoadScene (int sceneIndex) {
        SceneManager.LoadScene (sceneIndex);
    }

}