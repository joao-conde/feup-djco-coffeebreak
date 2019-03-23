using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseController : MonoBehaviour
{

    public GameObject pausePanel;
   

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Cancel")){
            TogglePause();
        }
    }

    public void TogglePause(){
        if(Time.timeScale == 0f){
            Time.timeScale = 1f;
            pausePanel.SetActive(false);
        }else{
            Time.timeScale  = 0f;
            pausePanel.SetActive(true);
        }
    }
}
