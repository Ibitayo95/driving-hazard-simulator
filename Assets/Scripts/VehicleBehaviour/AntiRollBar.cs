using UnityEngine;

namespace VehicleBehaviour
{
    public class AntiRollBar : MonoBehaviour
    {

        public WheelCollider WheelL;
        public WheelCollider WheelR;
        public float AntiRoll = 5000.0f;
        private Rigidbody carRigidBody;

        void Start()
        {
            carRigidBody = GetComponent<Rigidbody>();
        }

        void FixedUpdate()
        {

            WheelHit hit;
            float travelL = 1.0f;
            float travelR = 1.0f;


            bool groundedL = WheelL.GetGroundHit(out hit);
            if (groundedL)

                travelL = (-WheelL.transform.InverseTransformPoint(hit.point).y - WheelL.radius) / WheelL.suspensionDistance;



            bool groundedR = WheelR.GetGroundHit(out hit);
            if (groundedR)

                travelR = (-WheelR.transform.InverseTransformPoint(hit.point).y - WheelR.radius) / WheelR.suspensionDistance;



            float antiRollForce = (travelL - travelR) * AntiRoll;



            if (groundedL)
            {
                var transform1 = WheelL.transform;
                carRigidBody.AddForceAtPosition(transform1.up * -antiRollForce,
                    transform1.position);
            }

            if (groundedR)
            {
                var transform1 = WheelR.transform;
                carRigidBody.AddForceAtPosition(transform1.up * antiRollForce,
                    transform1.position);
            }
        }
    }
}
