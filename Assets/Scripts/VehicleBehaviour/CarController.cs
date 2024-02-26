using UnityEngine;

namespace VehicleBehaviour
{
    public class CarController : MonoBehaviour
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
        public float maxMotorTorque; // Maximum torque the motor can apply
        public float maxSteeringAngle = 30f; // Maximum steer angle the wheels can have
        public float drivingBrakeTorque = 300f; // The torque needed to gently brake to control car
        public float handBrakeTorque = 1000f; // brings car to a full stop
        public Vector3 centreOfMass;
        public float currentMotorTorque;
        private Rigidbody _rb;

        // Car route information
        private PlayerRouteWaypoint[] _playerRoute;
        private int _currentWaypointIndex;
        private bool _isDriving;
        private bool _hasBrakedToStop;
    

        private void Start()
        {
            _playerRoute = PlayerRoute.route;
            _rb = GetComponent<Rigidbody>();
            _rb.centerOfMass = centreOfMass;
            _wheelColliders = new[] { backRight, backLeft, frontLeft, frontRight };
            _transforms = new[] { backRightTransform, backLeftTransform, frontLeftTransform, frontRightTransform };

            if (SimulationConfig.CarPositionRandomised)
            {
                // set waypoint to random viable position
                _currentWaypointIndex = SimulationConfig.random.Next(0, _playerRoute.Length - 1);
                // set position
                transform.position = _playerRoute[_currentWaypointIndex].transform.position;
                // set rotation of car towards the next waypoint
                transform.LookAt(_playerRoute[_currentWaypointIndex + 1].transform);

            }
        }


        void FixedUpdate()
        {
            if (_playerRoute.Length == 0)
                return;

            ApplySteer();
            Drive();
            CheckWaypointDistance();
            CarControl();
            UpdateWheels(_wheelColliders, _transforms);
            AdjustMotorTorqueForIncline();
        }

        private void ApplySteer()
        {
            Vector3 relativeVector = transform.InverseTransformPoint(_playerRoute[_currentWaypointIndex].transform.position);
            float newSteer = (relativeVector.x / relativeVector.magnitude) * maxSteeringAngle;
            frontLeft.steerAngle = newSteer;
            frontRight.steerAngle = newSteer;
        }

        private void Drive()
        {
            if (!_isDriving)
            {
                frontLeft.motorTorque = maxMotorTorque;
                frontRight.motorTorque = maxMotorTorque;
                backLeft.motorTorque = maxMotorTorque;
                backRight.motorTorque = maxMotorTorque;
                _isDriving = true;
            } 
        }

        // Distance to next waypoint on route
        private void CheckWaypointDistance()
        {
            if (Vector3.Distance(transform.position, _playerRoute[_currentWaypointIndex].transform.position) < 5f)
            {
                _currentWaypointIndex = (_currentWaypointIndex + 1) % _playerRoute.Length;
            }
        }

        // Slows car down as it approaches various waypoints
        private void CarControl()
        {     
            bool isNearWaypoint = Vector3.Distance(transform.position, _playerRoute[_currentWaypointIndex].transform.position) <= 10f;
            bool isMovingFast = _rb.velocity.magnitude > 1.5;

            if (isNearWaypoint && isMovingFast)
            {
                ApplyDrivingBrake();
                return;
            } 
        }

        private void ApplyDrivingBrake()
        {
            if (_hasBrakedToStop) return;
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

            _hasBrakedToStop = true;
        }

        public void ReleaseBrake()
        {
            backLeft.brakeTorque = 0;
            backRight.brakeTorque = 0;
            frontLeft.brakeTorque = 0;
            frontRight.brakeTorque = 0;
            _hasBrakedToStop = false;
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

            for (var i = 0; i < wheels.Length; i++)
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
                currentMotorTorque = maxMotorTorque + 200f;
            }
            else // Level or downhill
            {
                currentMotorTorque = maxMotorTorque;
            }

            SetMotorTorque(currentMotorTorque);
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

    }
}
