using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using static SceneChanger;

public class TextScroller : MonoBehaviour {

    public float timeBetweenChars;

    private AudioSource typingSound;
    private bool complete = false;
    private string text;
    private Text display;
    private IEnumerator coroutine;
    private SceneChanger sc;

    void Start () {
        StartCoroutine (LoadGameAsync ()); //load game scene async while text displays
        sc = FindObjectOfType<SceneChanger> ();
        typingSound = GetComponent<AudioSource> ();
        display = gameObject.GetComponent<Text> ();
        text = display.text;
        display.text = "";
        coroutine = DisplayCharByChar ();
        StartCoroutine (coroutine);
    }

    void Update () {
        if (Input.GetButtonDown ("Interact")) {
            if (complete)
                sc.FadeToScene (2);
            else {
                typingSound.Stop ();
                StopCoroutine (coroutine);
                display.text = text;
                complete = true;
            }
        }
    }

    IEnumerator DisplayCharByChar () {
        typingSound.Play ();
        foreach (char c in text) {
            display.text += c;
            yield return new WaitForSeconds (timeBetweenChars);
        }
    }

    IEnumerator LoadGameAsync () {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync (2);
        asyncLoad.allowSceneActivation = false;
        while (!asyncLoad.isDone) {
            yield return null;
        }
    }

}