using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Traffic
{
    public class TrafficLightManager : MonoBehaviour
    {
        // assign in the editor
        public List<TrafficLight> trafficLights = new();

        private void Start()
        {
            foreach (TrafficLight light in trafficLights)
            {
                light.redLight = true;
            }

            StartCoroutine(TurnGreenThenRed(trafficLights));
        }

        private IEnumerator TurnGreenThenRed(List<TrafficLight> trafficLights)
        {
            while (true)
            {
                foreach (TrafficLight trafficLight in trafficLights)
                {
                    trafficLight.amberLight = true;
                    yield return new WaitForSecondsRealtime(1);
                    trafficLight.greenLight = true;
                    yield return new WaitForSecondsRealtime(5);
                    trafficLight.amberLight = true;
                    yield return new WaitForSecondsRealtime(1);
                    trafficLight.redLight = true;
                }
            }
        }
    }
}