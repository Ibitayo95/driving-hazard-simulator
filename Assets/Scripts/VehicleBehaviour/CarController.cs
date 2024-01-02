using System.Collections;
using System.Collections.Generic;
using PolyPerfect.City;
using UnityEngine;

public class CarController : MonoBehaviour
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

    // Car obstacle detection and intelligence
    public PlayerCarAI carAI;

    // Car specs
    public float maxMotorTorque; // Maximum torque the motor can apply
    public float maxSteeringAngle = 30f; // Maximum steer angle the wheels can have
    public float drivingBrakeTorque = 300f; // The torque needed to gently brake to control car
    public float handBrakeTorque = 1000f; // brings car to a full stop
    public Vector3 centreOfMass;
    public float currentMotorTorque;

    // Car route information
    private PlayerRouteWaypoint[] playerRoute;
    private int currentWaypointIndex = 0;
    private bool isDriving = false;
    


    private void Start()
    {
        playerRoute = PlayerRoute.route;
        GetComponent<Rigidbody>().centerOfMass = centreOfMass;
        carAI = GetComponent<PlayerCarAI>();
        wheelColliders = new WheelCollider[] { backRight, backLeft, frontLeft, frontRight };
        transforms = new Transform[] { backRightTransform, backLeftTransform, frontLeftTransform, frontRightTransform };

        if (SimulationConfig.CarPositionRandomised)
        {
            // set waypoint to random viable position
            currentWaypointIndex = SimulationConfig.random.Next(0, playerRoute.Length);
            // set position
            transform.position = playerRoute[currentWaypointIndex].transform.position;
        }
    }


    void FixedUpdate()
    {
        if (playerRoute.Length == 0)
            return;

        ApplySteer();
        Drive();
        CheckWaypointDistance();
        CarControl();
        UpdateWheels(wheelColliders, transforms);
        AdjustMotorTorqueForIncline();
    }

    private void ApplySteer()
    {
        Vector3 relativeVector = transform.InverseTransformPoint(playerRoute[currentWaypointIndex].transform.position);
        float newSteer = (relativeVector.x / relativeVector.magnitude) * maxSteeringAngle;
        frontLeft.steerAngle = newSteer;
        frontRight.steerAngle = newSteer;
    }

    private void Drive()
    {
        if (!isDriving)
        {
            frontLeft.motorTorque = maxMotorTorque;
            frontRight.motorTorque = maxMotorTorque;
            backLeft.motorTorque = maxMotorTorque;
            backRight.motorTorque = maxMotorTorque;
            isDriving = true;
        } 
    }

    // Distance to next waypoint on route
    private void CheckWaypointDistance()
    {
        if (Vector3.Distance(transform.position, playerRoute[currentWaypointIndex].transform.position) < 5f)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % playerRoute.Length;
        }
    }

    // Keeps car smooth and steady, no accidents. Car stops when required.
    private void CarControl()
    {
        if (carAI.DetectObstacle(this)) return;

        bool isNearWaypoint = Vector3.Distance(transform.position, playerRoute[currentWaypointIndex].transform.position) <= 10f;
        bool isMovingFast = GetComponent<Rigidbody>().velocity.magnitude > 1.5;

        if (isNearWaypoint && isMovingFast)
        {
            ApplyDrivingBrake();
            return;
        }

        ReleaseBrake();
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
        backLeft.brakeTorque =  handBrakeTorque * 0.5f;
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

    private void SetMotorTorque(float torque)
    {
        frontLeft.motorTorque = torque;
        frontRight.motorTorque = torque;
        backLeft.motorTorque = torque;
        backRight.motorTorque = torque;
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

    // to help car have enough power to go up hills
    private void AdjustMotorTorqueForIncline()
    {
        float incline = Vector3.Dot(transform.forward, Vector3.up);

        if (incline > 0) // Uphill
        {
            currentMotorTorque = maxMotorTorque + 200f;
        }
        else // Level or downhill
        {
            currentMotorTorque = maxMotorTorque;
        }

        SetMotorTorque(currentMotorTorque);
    }



    // if human is hit, ragdoll physics occurs
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Humans"))
        {
            // Calculate the direction from the vehicle to the pedestrian
            Vector3 direction = collision.transform.position - transform.position;
            direction = direction.normalized;

            collision.gameObject.GetComponentInParent<RagdollActivator>().HitByVehicle(direction, 5f);
        }
    }


}
