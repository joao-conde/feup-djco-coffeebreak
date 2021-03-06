﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DoorController : MonoBehaviour {

    public BoxCollider2D mainCollider;
    public bool isOpen;
    public AudioClip openSound;
    public AudioClip closeSound;

    private bool isLoading;
    private Animator animator;
    private NavMeshObstacle navMesh;
    private AudioSource actionSound;
    private float initialVolume;

    public void Start () {
        isLoading = true;
        this.animator = GetComponent<Animator> ();
        actionSound = GetComponent<AudioSource> ();
        initialVolume = actionSound.volume;
        actionSound.volume = initialVolume * GameManager.instance.sfxMultiplier;
        navMesh = GetComponent<NavMeshObstacle> ();
        isOpen = !isOpen;
        Interact ();
        isLoading = false;
    }

    public void Interact () {
        animator.enabled = true;

        if (!isOpen) {
            isOpen = true;
            animator.SetBool ("isClosed", false);
            if (!isLoading) {
                actionSound.clip = openSound;
                actionSound.Play ();

            }
            navMesh.enabled = false;
            gameObject.layer = LayerMask.NameToLayer ("Ignore Raycast");
            mainCollider.enabled = false;
        } else {
            isOpen = false;
            if (!isLoading) {
                actionSound.clip = closeSound;
                actionSound.Play ();
            }
            animator.SetBool ("isClosed", true);
            gameObject.layer = LayerMask.NameToLayer ("Default");
            navMesh.enabled = true;
            mainCollider.enabled = true;
        }
    }
}