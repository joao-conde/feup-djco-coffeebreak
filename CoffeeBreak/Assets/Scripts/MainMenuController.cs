using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour {

    public void Play () {
        SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex + 1);
    }

    public void Quit () {
        Debug.Log ("Quit only works in a compiled game, not in editor, but trust!");
        Application.Quit ();
    }

}