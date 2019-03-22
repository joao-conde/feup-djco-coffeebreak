using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static Player;

public class GuardController : AIController {

    public Light flashlight;

    public float viewDistance = 3f;

    
    public float rotationSpeed;
    public float awerenessRadius;
    public float coinPickUpRadius;

    public GameObject alertIcon;

    private GameObject targetCoin = null;

    private AudioSource footsteps;

    private AudioSource alert;

    private Renderer alertRenderer;
    private float stopThreshold = 0.20f;

    private float footstepsInitialVolume;

    private float alertInitialVolume;

    private bool spottedPlayer = false;
    private Animator animator;

    protected override void Start () {
        base.Start ();
        animator = GetComponent<Animator> ();

        footsteps = gameObject.GetComponents<AudioSource> ()[0];
        footstepsInitialVolume = footsteps.volume;
        footsteps.volume = footstepsInitialVolume * GameManager.instance.sfxMultiplier;
        alert = gameObject.GetComponents<AudioSource>()[1];
        alertInitialVolume = alert.volume;
        alert.volume = alertInitialVolume * GameManager.instance.sfxMultiplier;
        alertRenderer = alertIcon.GetComponent<Renderer> ();
        alertRenderer.enabled = false;
    }

    protected override void Update () {
        base.Update ();

        if (target != null) {
            if (!footsteps.isPlaying) {
                footsteps.Play ();
            }
        } else {
            footsteps.Stop ();
        }
        Vector3 direction = Vector3.Normalize (agent.velocity);
        flashlight.transform.forward = Vector3.RotateTowards (flashlight.transform.forward, direction, rotationSpeed * Time.deltaTime, 0.0f);
        HandleMovement ();
        HandleDistraction ();
        FieldOfView ();
    }

    protected void FieldOfView () {
        RaycastHit2D[] hit = Physics2D.RaycastAll ((Vector2) gameObject.transform.position, ((Vector2) flashlight.transform.forward.normalized), viewDistance);
        Debug.DrawRay (gameObject.transform.position, ((Vector2) flashlight.transform.forward.normalized * viewDistance));
        if (hit.Length > 1) {
            if (hit[1].collider.CompareTag ("Player") && !spottedPlayer) {
                spottedPlayer = true;
                Player player = (Player) hit[1].collider.gameObject.GetComponent ("Player");
                alert.Play();
                StartCoroutine (HandleSeen (player));
            }
        }
    }

    private void HandleMovement () {

        if (agent.velocity.y > 0 && agent.velocity.x > 0 && agent.velocity.y > agent.velocity.x) {
            animator.SetBool ("guardRight", false);
            animator.SetBool ("guardLeft", false);
            return;
        } else if (agent.velocity.y < 0 && agent.velocity.x < 0 && agent.velocity.y < agent.velocity.x) {
            animator.SetBool ("guardRight", false);
            animator.SetBool ("guardLeft", false);
            return;
        } else if (agent.velocity.y == 0 && agent.velocity.x == 0) {
            animator.SetBool ("guardRight", false);
            animator.SetBool ("guardLeft", false);
            return;
        }

        if (agent.velocity.x < 0) {
            animator.SetBool ("guardRight", false);
            animator.SetBool ("guardLeft", true);
        } else if (agent.velocity.x > 0) {
            animator.SetBool ("guardLeft", false);
            animator.SetBool ("guardRight", true);
        }
    }

    private void HandleDistraction () {
        Debug.DrawLine (transform.position, transform.position + new Vector3 (awerenessRadius, awerenessRadius, 0)); //comment for test
        Collider2D[] playerRadius = Physics2D.OverlapCircleAll (transform.position, awerenessRadius);
        foreach (Collider2D col in playerRadius) {
            if (col.tag == "Player") {
                Player playerController = (Player) col.gameObject.GetComponent ("Player");
                if (!playerController.IsStealth () && !spottedPlayer) {
                    spottedPlayer = true;
                    alert.Play();
                    StartCoroutine (HandleSeen (playerController));
                }
            }
        }

        Debug.DrawLine (transform.position, transform.position + new Vector3 (coinPickUpRadius, coinPickUpRadius, 0)); //comment for test
        Collider2D[] thrownCoinRadius = Physics2D.OverlapCircleAll (transform.position, coinPickUpRadius);
        foreach (Collider2D col in thrownCoinRadius) {
            if (col.tag == "ThrownCoin" && targetCoin == null) {
                targetCoin = col.gameObject;
            } 
        }

        if (targetCoin != null) {
            Rigidbody2D rbCoin = targetCoin.GetComponent<Rigidbody2D> ();

            //stopped coin
            if (rbCoin.velocity.x <= stopThreshold && rbCoin.velocity.y <= stopThreshold) {
                target = rbCoin.position;
            }

            //on coin
            if (agent.remainingDistance < 1f) {
                Destroy (targetCoin);
                targetCoin = null;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other){
        if(other.gameObject.tag == "Player" && !spottedPlayer){
                spottedPlayer = true;
                Player player = (Player) other.collider.gameObject.GetComponent ("Player");
                alert.Play();
                StartCoroutine (HandleSeen (player));
        }
    }

    private IEnumerator HandleSeen (Player player) {
        StartCoroutine (DoBlinks (1.5f, 0.2f));
        target = player.gameObject.transform.position;
        yield return new WaitForSeconds (0.5f);
        GotoNextPoint ();
        player.looseLife ();
        player.gameObject.transform.position = player.respawnPoint;
        spottedPlayer = false;
    }

    private IEnumerator DoBlinks (float duration, float blinkTime) {
        while (duration >= 0f) {
            duration -= (Time.deltaTime + blinkTime);
            alertRenderer.enabled = !alertRenderer.enabled;
            yield return new WaitForSeconds (blinkTime);
        }
        alertRenderer.enabled = false;
        spottedPlayer = false;
    }
}