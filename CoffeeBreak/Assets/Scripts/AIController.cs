using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIController : MonoBehaviour
{
    
    public Transform[] points;

    

    protected int destPoint = 0;
    protected NavMeshAgent agent;
    protected bool moveAI;
    protected Vector3 target;
    protected Vector3 direction;
    protected Vector3 origin;

    
    // Start is called before the first frame update
    protected virtual void Start()
    {
       
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        
        moveAI = false;
        origin = transform.position;
        
        GotoNextPoint();
        
    }

    protected void GotoNextPoint()
    {
        // Returns if no points have been set up
        if (points.Length == 0)
            return;

        // Set the agent to go to the currently selected destination.
        target = points[destPoint].position;
        Debug.Log(points[destPoint].position);

        // Choose the next point in the array as the destination,
        // cycling to the start if necessary.
        destPoint = (destPoint + 1) % points.Length;
        moveAI = true;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        
        // Choose the next destination point when the agent gets
        // close to the current one.
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            GotoNextPoint();
            
        }

        if (target != null)
        {
            agent.SetDestination(target);
        }
            
        if (Input.GetButtonDown("Fire2"))
        {
            target = Input.mousePosition;
            Ray ray=  Camera.main.ScreenPointToRay(target);
            target = ray.origin;
            moveAI = true;
            
        }

        if (Vector3.Distance(target, transform.position) < agent.stoppingDistance)
        {
            agent.SetDestination(transform.position);
        }
    }
}
