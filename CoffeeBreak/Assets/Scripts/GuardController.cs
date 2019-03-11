using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardController : AIController {

    public Light flashlight;
    public float rotationSpeed;
    public float awerenessRadius;

    private GameObject targetCoin = null;
    private float stopThreshold = 0.20f;


    protected override void Start () {
        base.Start ();
    }

    protected override void Update () {
        base.Update ();
        Vector3 direction = Vector3.Normalize (agent.velocity);
        flashlight.transform.forward = Vector3.RotateTowards (flashlight.transform.forward, direction, rotationSpeed * Time.deltaTime, 0.0f);
        HandleDistraction();
    }

    protected void FixedUpdate () {
        RaycastHit2D hit = Physics2D.Raycast((Vector2)gameObject.transform.position+(Vector2)flashlight.transform.forward.normalized,((Vector2)flashlight.transform.forward.normalized), 4.5f);
        Debug.DrawLine ((Vector2)gameObject.transform.position + (Vector2)flashlight.transform.forward.normalized, (Vector2)gameObject.transform.position + (Vector2)flashlight.transform.forward.normalized * 4.5f);
        if(hit.collider != null){
            if(hit.collider.CompareTag("Player")){
                //Debug.Log("Saw player");
            }
        }
    }

    //TODO: avoid catching coins not thrown - layerMask
    private void HandleDistraction(){
        Debug.DrawLine(transform.position, transform.position + new Vector3(awerenessRadius, awerenessRadius, 0)); //comment for test
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, awerenessRadius);
        foreach(Collider2D col in hitColliders){
            if(col.tag == "Coin" && targetCoin == null){ 
                targetCoin = col.gameObject;
            }
        }

        if (targetCoin != null) {
            Rigidbody2D rbCoin = targetCoin.GetComponent<Rigidbody2D> ();

            //stopped coin
            if (rbCoin.velocity.x <= stopThreshold && rbCoin.velocity.y <= stopThreshold) {
                agent.SetDestination (rbCoin.position);
            }

            //on coin
            if (agent.remainingDistance < 1f) {
                Destroy (targetCoin);
                targetCoin = null;
            }
        }
    }
}