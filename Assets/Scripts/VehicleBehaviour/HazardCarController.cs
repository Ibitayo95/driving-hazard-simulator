using System.Collections;
using System.Collections.Generic;
using UnityEngine.Serialization;
using UnityEngine;

/*
 * Variation on the player's car controller class. Main differences are the lack
 * of slowing down behaviour and the following of a special short hazard route.
 * Hazard waypoints will be put inside the car's gameobject.
 */
public class HazardCarController : MonoBehaviour, IHazardObject
{
    // Hazard identifiers
    [SerializeField] private string _name;
    public string Name => _name;
    [SerializeField] private int _chanceOfOccuring;
    public int ChanceOfOccuring => _chanceOfOccuring;
    [SerializeField] private float _hazardOffsetTime;
    public float HazardOffsetTime => _hazardOffsetTime;
    [SerializeField] private HazardType _hazardType;
    public HazardType Type => _hazardType;

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
    public bool isReversing;
    private int currentWaypointIndex = 0;
    private bool hazardActivated = false;



    private void Start()
    {
        GetComponent<Rigidbody>().centerOfMass = centreOfMass;
        wheelColliders = new WheelCollider[] { backRight, backLeft, frontLeft, frontRight };
        transforms = new Transform[] { backRightTransform, backLeftTransform, frontLeftTransform, frontRightTransform };
        _chanceOfOccuring = SimulationConfig.random.Next(0, 50);
    }


    void FixedUpdate()
    {
        if (waypoints.Length == 0) return;
        else if (!hazardActivated) return;
        // when we get to the end of the route, we deactivate the hazard and return
        else if (currentWaypointIndex == waypoints.Length)
        {
            hazardActivated = false;
            SwitchOffEngine();
            return;
        }
        else
        {
            ApplySteer();
            Drive();
            CheckWaypointDistance();
            UpdateWheels(wheelColliders, transforms);
        }
        
    }

    [ContextMenu("Activate")]
    public void ActivateHazard()
    {
        hazardActivated = true;
    }

    [ContextMenu("Deactivate")]
    public void DeactivateHazard()
    {
        hazardActivated = false;
        Destroy(gameObject, 3.0f);
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
        if (isReversing)
        {
            frontLeft.motorTorque = -maxMotorTorque * 0.75f;
            frontRight.motorTorque = -maxMotorTorque * 0.75f;
            backLeft.motorTorque = -maxMotorTorque * 0.75f;
            backRight.motorTorque = -maxMotorTorque * 0.75f;
        }
        else
        {
            frontLeft.motorTorque = maxMotorTorque;
            frontRight.motorTorque = maxMotorTorque;
            backLeft.motorTorque = maxMotorTorque;
            backRight.motorTorque = maxMotorTorque;
        }

    }

    // Distance to next waypoint on route
    private void CheckWaypointDistance()
    {

        if (Vector3.Distance(transform.position, waypoints[currentWaypointIndex].position) < 5f)
        {
            currentWaypointIndex++;
        }
    }

    public void SwitchOffEngine()
    {
        frontLeft.motorTorque = 0;
        frontRight.motorTorque = 0;
        backLeft.motorTorque = 0;
        backRight.motorTorque = 0;
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

    // to visualise the car's waypoints/route
    private void OnDrawGizmos()
    {
        if (waypoints == null || waypoints.Length == 0) return;
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