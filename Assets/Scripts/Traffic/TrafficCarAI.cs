using System;
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
            throw new NotImplementedException();
        }
        
        private void OnTriggerExit(Collider other)
        {
            throw new NotImplementedException();
        }
    }
}