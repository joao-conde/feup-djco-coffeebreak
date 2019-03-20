using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TipController : MonoBehaviour
{
    public GameObject dialogBox;
    private bool isNear;
    
    public void handleView(){
        if(isNear){
            isNear = false;
            dialogBox.SetActive(false);
        }else{
            isNear = true;
            dialogBox.SetActive(true);
        }
    }

}
