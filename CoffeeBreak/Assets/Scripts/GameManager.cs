using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    public static GameManager instance = null;
    public int initialPlayerCoins;

    private void Awake () {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy (gameObject);

        DontDestroyOnLoad (gameObject);
    }

    public void Start(){
        StartCoroutine (LoadGameAsync ());
    }

    public void EndGame () {
        LoadScene (2);
    }

    public void WinGame () {
        LoadScene (3);
    }

    private void LoadScene (int sceneIndex) {
        SceneManager.LoadScene (sceneIndex);
    }

    IEnumerator LoadGameAsync () {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync (1);
        asyncLoad.allowSceneActivation = false;
        while (!asyncLoad.isDone) {
            yield return null;
        }
    }

}