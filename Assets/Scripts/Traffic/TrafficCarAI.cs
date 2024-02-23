using System;
using UnityEngine;
using VehicleBehaviour;

namespace Traffic
{
    public class TrafficCarAI : MonoBehaviour
    {
        public TrafficCarController TrafficCar;
        public CarController UserCar;

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