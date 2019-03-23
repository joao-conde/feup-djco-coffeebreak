using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static TrashBinController;

public class JanitorController : AIController {

    public float awerenessRadius;

    private GameObject targetBin = null;
    private Animator animator;

    protected override void Start () {
        base.Start ();
        animator = GetComponent<Animator> ();
    }

    protected override void Update () {
        base.Update ();
        HandleMovement ();
        HandleTrashBins ();
    }

    private void HandleMovement () {

        if (agent.velocity.y > 0 && agent.velocity.x > 0 && agent.velocity.y > agent.velocity.x) {
            animator.SetBool ("janitorRight", false);
            animator.SetBool ("janitorLeft", false);
            return;
        } else if (agent.velocity.y < 0 && agent.velocity.x < 0 && agent.velocity.y < agent.velocity.x) {
            animator.SetBool ("janitorRight", false);
            animator.SetBool ("janitorLeft", false);
            return;
        } else if (agent.velocity.y == 0 && agent.velocity.x == 0) {
            animator.SetBool ("janitorRight", false);
            animator.SetBool ("janitorLeft", false);
            return;
        }

        if (agent.velocity.x < 0) {
            animator.SetBool ("janitorRight", false);
            animator.SetBool ("janitorLeft", true);
        } else if (agent.velocity.x > 0) {
            animator.SetBool ("janitorLeft", false);
            animator.SetBool ("janitorRight", true);
        }
    }

    private void HandleTrashBins () {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll (transform.position, awerenessRadius);
        foreach (Collider2D col in hitColliders) {
            if (col.tag == "TrashBin") {
                TrashBinController bin = (TrashBinController) col.gameObject.GetComponent ("TrashBinController");
                if (bin.IsOnFloor () && targetBin == null) {
                    targetBin = col.gameObject;
                }
            }
        }

        if (targetBin != null) {
            agent.SetDestination (targetBin.GetComponent<Transform> ().position);

            if (agent.remainingDistance < 1f) { //on bin, pick it up m' lady
                TrashBinController binController = (TrashBinController) targetBin.GetComponent ("TrashBinController");
                binController.PickupBin ();
                targetBin = null;
            }
        }
    }
}