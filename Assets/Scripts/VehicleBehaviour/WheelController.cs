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



    public List<Transform> waypoints;
    public float maxMotorTorque = 500f; // Maximum torque the motor can apply
    public float maxSteeringAngle = 30f; // Maximum steer angle the wheels can have
    public float brakeTorque = 30000f; // The torque that will be applied when we need the car to stop

    private int currentWaypointIndex = 0;

    void FixedUpdate()
    {
        if (waypoints.Count == 0)
            return;

        ApplySteer();
        Drive();
        CheckWaypointDistance();
        Braking();
        UpdateWheel(frontLeft, frontLeftTransform);
        UpdateWheel(backRight, backRightTransform);
        UpdateWheel(frontRight, frontRightTransform);
        UpdateWheel(backLeft, backLeftTransform);
    }

    private void ApplySteer()
    {
        Vector3 relativeVector = transform.InverseTransformPoint(waypoints[currentWaypointIndex].position);
        float newSteer = (relativeVector.x / relativeVector.magnitude) * maxSteeringAngle;
        frontLeft.steerAngle = newSteer;
        frontRight.steerAngle = newSteer;
    }

    private void Drive()
    {
        frontLeft.motorTorque = maxMotorTorque;
        frontRight.motorTorque = maxMotorTorque;
        backLeft.motorTorque = maxMotorTorque;
        backRight.motorTorque = maxMotorTorque;
    }

    private void CheckWaypointDistance()
    {
        if (Vector3.Distance(transform.position, waypoints[currentWaypointIndex].position) < 5f)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Count;
        }
    }

    private void Braking()
    {
        if (Vector3.Distance(transform.position, waypoints[currentWaypointIndex].position) <= 10f)
        {
            ApplyBrake();
        }
        else
        {
            ReleaseBrake();
        }
    }

    private void ApplyBrake()
    {
        // 70 % distribution of braking on the front tyres, 30 % on rear
        backLeft.brakeTorque = brakeTorque * 0.5f;
        backRight.brakeTorque = brakeTorque * 0.5f;
        frontLeft.brakeTorque = brakeTorque * 1.5f;
        frontRight.brakeTorque = brakeTorque * 1.5f;
    }

    private void ReleaseBrake()
    {
        backLeft.brakeTorque = 0;
        backRight.brakeTorque = 0;
        frontLeft.brakeTorque = 0;
        frontRight.brakeTorque = 0;
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
