using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    public static GameManager instance = null;
    public int initialPlayerCoins = 0;

    private bool loadedGame = false;

    private void Awake () {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy (gameObject);

        DontDestroyOnLoad (gameObject);
    }

    public void Update () {
        if (!loadedGame){
            loadedGame = true;
            StartCoroutine (LoadGameAsync ());
        }
    }

    public void EndGame () {
        //Invoke ("Restart", 2f);
        LoadScene (2);
    }

    public void WinGame () {
        //Invoke ("Restart", 2f);
        LoadScene (3);
    }

    private void LoadScene (int sceneIndex) {
        SceneManager.LoadScene (sceneIndex);
    }

    IEnumerator LoadGameAsync () {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync (1);
        asyncLoad.allowSceneActivation = false;
        while (!asyncLoad.isDone) {
            // Debug.Log(asyncLoad.progress * 100 + "%");
            yield return null;
        }
        // Debug.Log ("Finished loading game");
    }

}