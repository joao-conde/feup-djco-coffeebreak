using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardController : AIController

{

    public Light flashlight;
    public Vector3 velocity;
    
    // Start is called before the first frame update

    protected  void Start()
    {
        base.Start();
        velocity = agent.velocity;
        
        
    }


    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
       /* Vector3 moveDirection = agent.velocity;
      
        if(moveDirection != Vector3.zero)
        {
            Debug.Log("Ola");
            float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;

            Quaternion q = Quaternion.AngleAxis(angle, Vector3.right);
            flashlight.transform.rotation = Quaternion.Slerp(flashlight.transform.rotation, q, Time.deltaTime*5);
        }
       */
       
    }
}
