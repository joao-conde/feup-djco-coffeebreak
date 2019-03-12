using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    
    public float moveSpeed;
    public GameObject followTarget;

    private Vector3 targetPosition;

    private void Update () {
        targetPosition = new Vector3 (followTarget.transform.position.x, followTarget.transform.position.y, transform.position.z);
        transform.position = Vector3.Lerp (transform.position, targetPosition, moveSpeed * Time.deltaTime);
    }
}