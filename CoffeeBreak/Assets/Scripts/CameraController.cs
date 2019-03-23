using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public float moveSpeed;
    public GameObject followTarget;

    private Vector3 targetPosition;
    private AudioSource music;
    private float initialVolume;

    private void Start () {
        music = GetComponent<AudioSource> ();
        initialVolume = music.volume;
        music.volume = initialVolume * GameManager.instance.musicMultiplier;
    }

    private void Update () {
        music.volume = initialVolume * GameManager.instance.musicMultiplier;

        if (followTarget != null) {
            targetPosition = new Vector3 (followTarget.transform.position.x, followTarget.transform.position.y, transform.position.z);
            transform.position = Vector3.Lerp (transform.position, targetPosition + new Vector3 (0, 1, 0), moveSpeed * Time.deltaTime);
        }
    }
}