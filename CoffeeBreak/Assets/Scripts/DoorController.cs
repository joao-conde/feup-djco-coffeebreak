using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public bool isOpened;
    public BoxCollider2D mainCollider;
    

    public void Open()
    {
        if (!isOpened)
        {
            isOpened = true;
            mainCollider.enabled = false;
        }
        else
        {
            isOpened = false;
            mainCollider.enabled = true;
        }
    }
}
