using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JanitorController : AIController
{
    enum janitorState
    {
        patroling,
        cleaning,
    }

    private janitorState state;
    private void MakeDecision()
    {
        switch (state){
            case janitorState.patroling:
                Patrol();
                break;
            case janitorState.cleaning:
                Clean();
                break;
        }
    }
    // Start is called before the first frame update
    protected override void Start()
    {
        state = janitorState.patroling;
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        MakeDecision();

        Debug.Log(agent.hasPath);

        if (target != null)
        {
            agent.SetDestination(target);
        }

        if (Vector3.Distance(target, transform.position) < agent.stoppingDistance)
        {
            agent.SetDestination(transform.position);
        }
    }

    private void Clean()
    {

    }

    private void Patrol()
    {
        // Choose the next destination point when the agent gets
        // close to the current one.
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            GotoNextPoint();

        }
    }
}
