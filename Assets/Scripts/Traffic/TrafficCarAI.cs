using UnityEngine;
using VehicleBehaviour;

namespace Traffic
{
    public class TrafficCarAI : MonoBehaviour
    {
        public TrafficCarController trafficCar;
        public CarController userCar;

        private void OnTriggerEnter(Collider other)
        {
            if (userCar != null)
            {
                // if a hazard human - then no braking (to simulate accident)
                if (other.gameObject.layer == LayerMask.NameToLayer("Humans"))
                {
                    return;
                }
                
                userCar.ApplyHandBrake();
            }

            else if (trafficCar != null)
            {
                trafficCar.ApplyHandBrake();
            }
        }
        
        private void OnTriggerExit(Collider other)
        {
            if (userCar != null)
            {
                userCar.ReleaseBrake();
            }
            else if (trafficCar != null)
            {
                trafficCar.ReleaseBrake();
            }
        }
    }
}