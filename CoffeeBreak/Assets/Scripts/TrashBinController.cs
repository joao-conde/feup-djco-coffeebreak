using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashBinController : MonoBehaviour{

    private bool isOnFloor;

    public void Start(){
        // this.animator = GetComponent<Animator>();
        // animator.enabled = false;
        this.isOnFloor = false;
    }

    public void Interact(){
        if(!isOnFloor){
            isOnFloor = true;
            transform.Rotate(0, 0, -90);
        }
        else{
            isOnFloor = false;
            transform.Rotate(0, 0, 90);
        }
    }

}
