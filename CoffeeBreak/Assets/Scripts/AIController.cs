using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIController : MonoBehaviour
{
    private NavMeshAgent agent;
    private bool moveAI;
    private Vector3 target;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        moveAI = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (moveAI && target != null)
        {
            agent.SetDestination(target);
            Debug.Log(target);
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
