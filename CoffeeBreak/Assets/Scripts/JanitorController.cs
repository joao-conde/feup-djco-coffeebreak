using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static TrashBinController;

public class JanitorController : AIController
{
    
    public float awerenessRadius;
    private GameObject targetBin = null;
    

    protected override void Start(){
        base.Start();
    }

    protected override void Update(){
        base.Update();
        HandleTrashBins();
    }
    
    private void HandleTrashBins(){
        Debug.DrawLine(transform.position, transform.position + new Vector3(awerenessRadius, awerenessRadius, 0)); //comment for test
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, awerenessRadius);
        foreach(Collider2D col in hitColliders){
            if(col.tag == "TrashBin"){ 
                TrashBinController bin = (TrashBinController)col.gameObject.GetComponent("TrashBinController");
                if(bin.IsOnFloor() && targetBin == null){
                    targetBin = col.gameObject;
                }
            }
        }

        if(targetBin != null){
            agent.SetDestination(targetBin.GetComponent<Transform>().position);

            //on bin, pick it up m' lady
            if(agent.remainingDistance < 1f){
                TrashBinController bin = (TrashBinController)targetBin.GetComponent("TrashBinController");
                bin.PickupBin();
                targetBin = null;
            } 
        }
    }
}
