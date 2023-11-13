using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelController : MonoBehaviour
{
    [SerializeField] WheelCollider frontRight;
    [SerializeField] WheelCollider backRight;
    [SerializeField] WheelCollider frontLeft;
    [SerializeField] WheelCollider backLeft;

    [SerializeField] Transform backLeftTransform;
    [SerializeField] Transform frontRightTransform;
    [SerializeField] Transform frontLeftTransform;
    [SerializeField] Transform backRightTransform;



    public float acceleration = 500f;
    public float breakingForce = 300f;
    public float maxTurnAngle = 15f;


    private float currentAcceleration = 0f;
    private float currentBreakForce = 0f;
    private float currentTurnAngle = 0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        currentAcceleration = acceleration * Input.GetAxis("Vertical");

        if (Input.GetKey(KeyCode.Space)) // on some condition
        {
            currentBreakForce = breakingForce;
        }
        else
        {
            currentBreakForce = 0f;
        }

        // acceleration to front wheels (front wheel drive)
        frontRight.motorTorque = currentAcceleration;
        frontLeft.motorTorque = currentAcceleration;

        // braking to all wheels
        frontRight.brakeTorque = currentBreakForce;
        backRight.brakeTorque = currentBreakForce;
        frontLeft.brakeTorque = currentBreakForce;
        backLeft.brakeTorque = currentBreakForce;


        // steering
        currentTurnAngle = maxTurnAngle * Input.GetAxis("Horizontal");
        frontLeft.steerAngle = currentTurnAngle;
        frontRight.steerAngle = currentTurnAngle;

        // Update wheel meshes (so they actually turn with the car)
         UpdateWheel(frontLeft, frontLeftTransform);
         UpdateWheel(backRight, backRightTransform);
         UpdateWheel(frontRight, frontRightTransform);
         UpdateWheel(backLeft, backLeftTransform);
        
    }


    void UpdateWheel(WheelCollider wheel, Transform trans)
    {
        // Get wheel collider state
        Vector3 position;
        Quaternion rotation;
        wheel.GetWorldPose(out position, out rotation);

        // Set Wheel transform state
        trans.position = position;
        trans.rotation = rotation; 
    } 
}
