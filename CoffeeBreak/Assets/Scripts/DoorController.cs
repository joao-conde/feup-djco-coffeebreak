using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public BoxCollider2D mainCollider;
    
    private bool isOpen;
    private Animator animator;
    

    public void Start(){
        this.isOpen = false;
        this.animator = GetComponent<Animator>();
        animator.enabled = false;
    }

    public void Interact()
    {      
        animator.enabled = true;
        
        if (!isOpen)
        {
            isOpen = true;
            mainCollider.enabled = false;
            animator.SetBool("isClosed", false);
        }
        else
        {
            isOpen = false;
            mainCollider.enabled = true;
            animator.SetBool("isClosed", true);
        }
    }
}
