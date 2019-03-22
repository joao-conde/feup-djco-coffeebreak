using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashBinController : MonoBehaviour {

    private bool isOnFloor;
    private AudioSource sound;
    Animator m_Animator;

    public void Start () {
        this.isOnFloor = false;
        sound = GetComponent<AudioSource>();
        m_Animator = gameObject.GetComponent<Animator> ();
    }

    public void DropBin () {
        if (!isOnFloor) {
            isOnFloor = true;
            sound.Play();
            m_Animator.SetBool ("isOnFloor", true);
        }
    }

    public void PickupBin () {
        if (isOnFloor) {
            isOnFloor = false;
            m_Animator.SetBool ("isOnFloor", false);
        }
    }

    public bool IsOnFloor () {
        return isOnFloor;
    }
}