using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DoorController : MonoBehaviour {

    public BoxCollider2D mainCollider;

    public  bool isOpen;

    public AudioClip openSound;

    private bool isLoading;

    public AudioClip closeSound;
    private Animator animator;
    private NavMeshObstacle navMesh;

    private AudioSource actionSound;

    public void Start () {
        isLoading = true;
        this.animator = GetComponent<Animator> ();
        actionSound = GetComponent<AudioSource>();
        navMesh = GetComponent<NavMeshObstacle> ();
        isOpen = !isOpen;
        Interact();
        isLoading = false;
    }

    public void Interact () {
        animator.enabled = true;

        if (!isOpen) {
            isOpen = true;
            mainCollider.enabled = false;
            animator.SetBool ("isClosed", false);
            if(!isLoading){
                actionSound.clip = openSound;
                actionSound.Play();
                
            }
            navMesh.enabled = false;
            gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
        } else {
            isOpen = false;
            mainCollider.enabled = true;
            if(!isLoading){
                actionSound.clip = closeSound;
                actionSound.Play();
            }
            animator.SetBool ("isClosed", true);
            gameObject.layer = LayerMask.NameToLayer("Default");
            navMesh.enabled = true;

        }
    }
}