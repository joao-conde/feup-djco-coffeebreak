using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIController : MonoBehaviour
{
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
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        direction = agent.velocity.normalized;
       
        if (moveAI && target != null)
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
    }
}
