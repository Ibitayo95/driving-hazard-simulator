using System;
using UnityEngine;

namespace Traffic
{
    public class TrafficLightSensor : MonoBehaviour
    {
        public Material RedLight;
        public Material GreenLight;

        private void OnTriggerEnter(Collider other)
        {
            throw new NotImplementedException();
        }
    }
}