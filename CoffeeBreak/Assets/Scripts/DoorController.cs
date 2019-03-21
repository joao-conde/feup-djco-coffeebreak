using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DoorController : MonoBehaviour {

    public BoxCollider2D mainCollider;

    public  bool isOpen;

    public AudioClip openSound;

    public AudioClip closeSound;
    private Animator animator;
    private NavMeshObstacle navMesh;

    private AudioSource actionSound;

    public void Start () {
        this.animator = GetComponent<Animator> ();
        actionSound = GetComponent<AudioSource>();
        navMesh = GetComponent<NavMeshObstacle> ();
        isOpen = !isOpen;
        Interact();
    }

    public void Interact () {
        animator.enabled = true;

        if (!isOpen) {
            isOpen = true;
            mainCollider.enabled = false;
            animator.SetBool ("isClosed", false);
            actionSound.clip = openSound;
            actionSound.Play();
            navMesh.enabled = false;
        } else {
            isOpen = false;
            mainCollider.enabled = true;
            actionSound.clip = closeSound;
            actionSound.Play();
            animator.SetBool ("isClosed", true);
            navMesh.enabled = true;

        }
    }
}