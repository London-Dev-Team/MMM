using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlywheelController : MonoBehaviour
{
    [SerializeField]
    private WheelJoint2D flywheelWheelJoint;
    [SerializeField]
    private Rigidbody2D mainWheel;
    
    private float speed = 70f;
    
    // Start is called before the first frame update
    void Start()
    {
        mainWheel.inertia = 400f;
    }

    // Update is called once per frame
    void Update()
    {
        if (! flywheelWheelJoint.useMotor){
            if (Mathf.Abs(mainWheel.angularVelocity) > (speed + 1f)){
                flywheelWheelJoint.useMotor = true;
                JointMotor2D motorRef = flywheelWheelJoint.motor;
                motorRef.motorSpeed =  Mathf.Sign(mainWheel.angularVelocity) * -speed;
                flywheelWheelJoint.motor = motorRef;
            }
        }
    }
}
