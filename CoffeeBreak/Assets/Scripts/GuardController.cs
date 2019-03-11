using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardController : AIController{

    public Light flashlight;
    public Vector3 velocity;

    public GameObject targetCoin = null;
    private float stopThreshold = 0.20f;
    
    // Start is called before the first frame update

    protected override void Start(){ 
        base.Start();
        velocity = agent.velocity;
    }

    // Update is called once per frame
    protected override void Update()
    {
       base.Update();
       HandleDistraction();
    }


    //TODO: avoid catching coins not thrown
    private void HandleDistraction(){
        if(targetCoin != null){
            Rigidbody2D rbCoin = targetCoin.GetComponent<Rigidbody2D>();
            
            //stopped coin
            if(rbCoin.velocity.x <= stopThreshold && rbCoin.velocity.y <= stopThreshold){ 
                agent.SetDestination(rbCoin.position);
            }
            
            //on coin
            if(agent.remainingDistance < 1f){
                Destroy(targetCoin);
                targetCoin = null;
            } 
        }
    }
    
    

    private void OnTriggerStay2D(Collider2D collision) {
        if(collision.gameObject.CompareTag("Coin") && targetCoin == null){
            targetCoin = collision.gameObject;
        }
    }
}
