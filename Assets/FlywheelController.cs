using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlywheelController : MonoBehaviour
{
    [SerializeField]
    private WheelJoint2D flywheelWheelJoint;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // JointMotor2D motorRef = flywheelWheelJoint.motor;
        // motorRef.motorSpeed += 0.5f;
        // flywheelWheelJoint.motor = motorRef;
    }
}
