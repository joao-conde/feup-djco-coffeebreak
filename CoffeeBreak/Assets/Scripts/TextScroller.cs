using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextScroller : MonoBehaviour {

    public float timeBetweenChars;

    private string text;    
    private Text display;

    private IEnumerator coroutine;


    void Start () {
        display = gameObject.GetComponent<Text> (); 
        text = display.text;
        display.text = "";
        coroutine = DisplayCharByChar ();
        StartCoroutine(coroutine);  
    }

    void Update(){
        if(Input.GetButtonDown ("Interact")){
            StopCoroutine(coroutine);
            display.text = text;
        }
    }

    public IEnumerator DisplayCharByChar () {
        foreach (char c in text) {
            display.text += c;
            yield return new WaitForSeconds (timeBetweenChars);
        }
    }

}