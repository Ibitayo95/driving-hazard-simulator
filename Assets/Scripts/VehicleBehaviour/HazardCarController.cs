using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Variation on the player's car controller class. Main differences are the lack
 * of slowing down behaviour and the following of a special short hazard route.
 * Hazard waypoints will be put inside the car's gameobject.
 */
public class HazardCarController : MonoBehaviour
{
    [SerializeField] WheelCollider frontRight;
    [SerializeField] WheelCollider backRight;
    [SerializeField] WheelCollider frontLeft;
    [SerializeField] WheelCollider backLeft;
    private WheelCollider[] wheelColliders;



    [SerializeField] Transform backLeftTransform;
    [SerializeField] Transform frontRightTransform;
    [SerializeField] Transform frontLeftTransform;
    [SerializeField] Transform backRightTransform;
    private Transform[] transforms;

 

    // Car specs
    public float maxMotorTorque = 500f; // Maximum torque the motor can apply
    public float maxSteeringAngle = 30f; // Maximum steer angle the wheels can have
    public float drivingBrakeTorque = 300f; // The torque needed to gently brake to control car
    public float handBrakeTorque = 1000f; // brings car to a full stop
    public Vector3 centreOfMass;

    // Car route information
    public Transform[] waypoints;
    private int currentWaypointIndex = 0;
    private bool hazardActivated = false;



    private void Start()
    {
        GetComponent<Rigidbody>().centerOfMass = centreOfMass;
        wheelColliders = new WheelCollider[] { backRight, backLeft, frontLeft, frontRight };
        transforms = new Transform[] { backRightTransform, backLeftTransform, frontLeftTransform, frontRightTransform };
    }


    void FixedUpdate()
    {
        if (waypoints.Length == 0) return;
        if (!hazardActivated) return;
        // when we get to the end of the route, we deactivate the hazard and return
        if (currentWaypointIndex == waypoints.Length)
        {
            hazardActivated = false;
            return;
        }

        ApplySteer();
        Drive();
        CheckWaypointDistance();
        UpdateWheels(wheelColliders, transforms);
    }

    [ContextMenu("Activate")]
    public void ActivateHazard()
    {
        hazardActivated = true;
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

    // Distance to next waypoint on route
    private void CheckWaypointDistance()
    {

        if (Vector3.Distance(transform.position, waypoints[currentWaypointIndex].position) < 5f)
        {
            currentWaypointIndex++;
        }
    }



    public void ApplyDrivingBrake()
    {
        // 70 % distribution of braking on the front tyres, 30 % on rear

        backLeft.brakeTorque = drivingBrakeTorque * 0.5f;
        backRight.brakeTorque = drivingBrakeTorque * 0.5f;
        frontLeft.brakeTorque = drivingBrakeTorque * 1.5f;
        frontRight.brakeTorque = drivingBrakeTorque * 1.5f;


    }

    public void ApplyHandBrake()
    {
        // 70 % distribution of braking on the front tyres, 30 % on rear
        backLeft.brakeTorque = handBrakeTorque * 0.5f;
        backRight.brakeTorque = handBrakeTorque * 0.5f;
        frontLeft.brakeTorque = handBrakeTorque * 1.5f;
        frontRight.brakeTorque = handBrakeTorque * 1.5f;
    }

    public void ReleaseBrake()
    {
        backLeft.brakeTorque = 0;
        backRight.brakeTorque = 0;
        frontLeft.brakeTorque = 0;
        frontRight.brakeTorque = 0;
    }

    // Updates the visual appearance of wheels rotating
    void UpdateWheels(WheelCollider[] wheels, Transform[] trans)
    {

        for (int i = 0; i < wheels.Length; i++)
        {
            // Get wheel collider state
            Vector3 position;
            Quaternion rotation;
            wheels[i].GetWorldPose(out position, out rotation);

            // Set Wheel transform state
            trans[i].SetPositionAndRotation(position, rotation);
        }

    }

    // to visualise the pedestrian's waypoints/route
    private void OnDrawGizmos()
    {
        if (waypoints.Length == 0) return;
        foreach (Transform t in waypoints)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(t.position, 0.5f);
        }

        Gizmos.color = Color.red;
        // lines that connect the route of waypoints
        for (int i = 0; i < waypoints.Length - 1; i++)
        {
            Gizmos.DrawLine(waypoints[i].position, waypoints[i + 1].position);
        }
    }
}

