using UnityEngine;

namespace Traffic
{
    public class TrafficCarController : MonoBehaviour
{
    [SerializeField] WheelCollider frontRight;
    [SerializeField] WheelCollider backRight;
    [SerializeField] WheelCollider frontLeft;
    [SerializeField] WheelCollider backLeft;
    private WheelCollider[] _wheelColliders;

    [SerializeField] Transform backLeftTransform;
    [SerializeField] Transform frontRightTransform;
    [SerializeField] Transform frontLeftTransform;
    [SerializeField] Transform backRightTransform;
    private Transform[] _transforms;

  
    // Car specs
    public float engineTorque;
    private readonly float _minEngineTorque = 130f;
    private readonly float _maxEngineTorque = 145f;
    private readonly float _maxSteeringAngle = 45f; // Maximum steer angle the wheels can have
    private readonly float _drivingBrakeTorque = 300f; // The torque needed to gently brake to control car
    private readonly float _handBrakeTorque = 30000f; // brings car to a full stop
    public Vector3 centreOfMass;
    private float _currentMotorTorque;
    private Rigidbody _rb;

    // Car route information
    public Transform[] waypoints;
    private int _currentWaypointIndex;
    private bool _isDriving;
    private bool _isStopped;
   
    


    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.centerOfMass = centreOfMass;
        _wheelColliders = new[] { backRight, backLeft, frontLeft, frontRight };
        _transforms = new[] { backRightTransform, backLeftTransform, frontLeftTransform, frontRightTransform };

        // set random traffic car position on route
        SetRandomPositionOnRoute();

        // set random speed for traffic car
        engineTorque = Random.Range(_minEngineTorque, _maxEngineTorque);
        ReleaseBrake();
        Drive();

        }


    void FixedUpdate()
    {
        if (waypoints.Length == 0)
            return;

        ApplySteer();
        Drive();
        CheckWaypointDistance();
        UpdateWheels(_wheelColliders, _transforms);
        AdjustMotorTorqueForIncline();
    }

    private void SetRandomPositionOnRoute()
    {
	    bool positionSet = false;
        
	    while (!positionSet)
	    {
		    _currentWaypointIndex = Random.Range(0, waypoints.Length - 1);
            Vector3 possiblePosition = waypoints[_currentWaypointIndex].transform.position;
            Collider[] colliders = Physics.OverlapSphere(possiblePosition, 20f, LayerMask.NameToLayer("Car"));
		    // if there are no traffic vehicles occupying the space then move car there
		    if (colliders.Length == 0) 
		    {
			    transform.position = possiblePosition;
                transform.LookAt(waypoints[_currentWaypointIndex + 1].transform);
				positionSet = true;
		    }
		    // otherwise we loop again selecting a new random position
	    }
    }

    private void ApplySteer()
    {
        Vector3 relativeVector = transform.InverseTransformPoint(waypoints[_currentWaypointIndex].transform.position);
        float newSteer = (relativeVector.x / relativeVector.magnitude) * _maxSteeringAngle;
        frontLeft.steerAngle = newSteer;
        frontRight.steerAngle = newSteer;
    }

    private void Drive()
    {
        if (!_isDriving)
        {
            frontLeft.motorTorque = engineTorque;
            frontRight.motorTorque = engineTorque;
            backLeft.motorTorque = engineTorque;
            backRight.motorTorque = engineTorque;

            _currentMotorTorque = engineTorque;
            _isDriving = true;
        } 
    }

    // Distance to next waypoint on route
    private void CheckWaypointDistance()
    {
        if (Vector3.Distance(transform.position, waypoints[_currentWaypointIndex].transform.position) < 5f)
        {
            _currentWaypointIndex = (_currentWaypointIndex + 1) % waypoints.Length;
        }
    }


    private void ApplyDrivingBrake()
    {
        if (_isStopped) return;
        // 70 % distribution of braking on the front tyres, 30 % on rear
        backLeft.brakeTorque = _drivingBrakeTorque * 0.5f;
        backRight.brakeTorque = _drivingBrakeTorque * 0.5f;
        frontLeft.brakeTorque = _drivingBrakeTorque * 1.5f;
        frontRight.brakeTorque = _drivingBrakeTorque * 1.5f;
    }

    public void ApplyHandBrake()
    {
        // 70 % distribution of braking on the front tyres, 30 % on rear
        backLeft.brakeTorque =  _handBrakeTorque * 0.5f;
        backRight.brakeTorque = _handBrakeTorque * 0.5f;
        frontLeft.brakeTorque = _handBrakeTorque * 1.5f;
        frontRight.brakeTorque = _handBrakeTorque * 1.5f;
        _isStopped = true;
    }

    public void ReleaseBrake()
    {
        backLeft.brakeTorque = 0;
        backRight.brakeTorque = 0;
        frontLeft.brakeTorque = 0;
        frontRight.brakeTorque = 0;
        _isStopped = false;
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
            wheels[i].GetWorldPose(out var position, out var rotation);

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
            _currentMotorTorque = engineTorque + 200f;
        }
        else // Level or downhill
        {
            _currentMotorTorque = engineTorque;
        }

        SetMotorTorque(_currentMotorTorque);
    }



    // if human is hit, ragdoll physics occurs
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Humans"))
        {
            // Calculate the direction from the vehicle to the pedestrian
            Vector3 direction = collision.transform.position - transform.position;
            direction = direction.normalized;

            collision.gameObject.GetComponentInParent<RagdollActivator>().HitByVehicle(direction, 5f);
        }
    }
    
    // to visualise the traffic car's waypoints/route
    private void OnDrawGizmos()
    {
        if (waypoints == null || waypoints.Length == 0) return;
        foreach (var t in waypoints)
        {
            Gizmos.color = Color.white;
            Gizmos.DrawWireCube(t.position, new Vector3(1, 1, 1));
        }

        Gizmos.color = Color.cyan;
        // lines that connect the route of waypoints
        for (var i = 0; i < waypoints.Length - 1; i++)
        {
            Gizmos.DrawLine(waypoints[i].position, waypoints[i + 1].position);
        }
    }
}
}

