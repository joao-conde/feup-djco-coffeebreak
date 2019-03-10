using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DoorController : MonoBehaviour
{
    public BoxCollider2D mainCollider;
    
    private bool isOpen;
    private Animator animator;
    private NavMeshObstacle navMesh;
    

    public void Start(){
        this.isOpen = false;
        this.animator = GetComponent<Animator>();
        animator.enabled = false;
        navMesh = GetComponent<NavMeshObstacle>();
    }

    public void Interact()
    {      
        animator.enabled = true;
        
        if (!isOpen)
        {
            isOpen = true;
            mainCollider.enabled = false;
            animator.SetBool("isClosed", false);
            navMesh.enabled = false;
        }
        else
        {
            isOpen = false;
            mainCollider.enabled = true;
            animator.SetBool("isClosed", true);
            navMesh.enabled = true;

        }
    }
}
