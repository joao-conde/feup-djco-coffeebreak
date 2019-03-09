using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public bool isOpen;
    public BoxCollider2D mainCollider;
    

    public void Open()
    {
        if (!isOpen)
        {
            isOpen = true;
            mainCollider.enabled = false;
        }
        else
        {
            isOpen = false;
            mainCollider.enabled = true;
        }
    }
}
