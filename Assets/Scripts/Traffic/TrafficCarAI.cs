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
                CarAdvanceTrafficLight(other, cc: userCar);
            }

            else if (trafficCar != null)
            {
                CarAdvanceTrafficLight(other, tc: trafficCar);
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (userCar != null)
            {
                CarAdvanceTrafficLight(other, cc: userCar);
            }
            else if (trafficCar != null)
            {
                CarAdvanceTrafficLight(other, tc: trafficCar);
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
        
        private void CarAdvanceTrafficLight(Collider other, TrafficCarController tc = null, CarController cc = null)
        {
            bool user = cc != null;
            bool traffic = tc != null;
            
            if (other.gameObject.layer == LayerMask.NameToLayer("Traffic"))
            {
                var trafficLight = other.gameObject.GetComponent<TrafficLight>();
                if (trafficLight.greenLight)
                {
                    if (user)
                    {
                        cc.ReleaseBrake();
                    }
                    else if (traffic)
                    {
                        tc.ReleaseBrake();
                    }
                }
                else
                {
                    if (user)
                    {
                        cc.ApplyHandBrake();
                    }
                    else if (traffic)
                    {
                        tc.ApplyHandBrake();
                    }
                }
            }
        }
    }
}