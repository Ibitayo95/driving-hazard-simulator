using System;
using UnityEngine;

namespace Traffic
{
    public class TrafficLightSensor : MonoBehaviour
    {
        public Material redLight;
        public Material greenLight;

        private void OnTriggerEnter(Collider other)
        {
            throw new NotImplementedException();
        }
    }
}