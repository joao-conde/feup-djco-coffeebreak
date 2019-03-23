using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIController : MonoBehaviour {

    public Transform[] points;

    protected int destPoint = 0;
    protected NavMeshAgent agent;
    protected Vector3 target;
    protected Vector3 direction;
    protected Vector3 origin;

    protected virtual void Start () {
        agent = GetComponent<NavMeshAgent> ();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        origin = transform.position;

        GotoNextPoint ();
    }

    protected void GotoNextPoint () {
        if (points.Length == 0) return;
        target = points[destPoint].position;
        destPoint = (destPoint + 1) % points.Length;
    }

    protected virtual void Update () {
        if (!agent.pathPending && agent.remainingDistance <= 1.2f) {
            GotoNextPoint ();
        }

        if (target != null) {
            agent.SetDestination (target);
        }

        if (Input.GetButtonDown ("Fire2")) {
            target = Input.mousePosition;
            Ray ray = Camera.main.ScreenPointToRay (target);
            target = ray.origin;
        }

        if (Vector3.Distance (target, transform.position) < agent.stoppingDistance) {
            agent.SetDestination (transform.position);
        }
    }
}