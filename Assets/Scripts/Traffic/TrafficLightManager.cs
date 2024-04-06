using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Traffic
{
    public class TrafficLightManager : MonoBehaviour
    {
        // assign in the editor
        public List<TrafficLight> trafficLights = new();
        public int intervalTime = 8;

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
            if (trafficLights.Count == 1)
            {
                while(true)
                {
                    foreach(TrafficLight trafficLight in trafficLights)
                    {
                        trafficLight.redLight = true;
                        trafficLight.amberLight = false;
                        trafficLight.greenLight = false;
                        yield return new WaitForSecondsRealtime(3);
                        trafficLight.amberLight = true;
                        trafficLight.redLight = false;
                        yield return new WaitForSecondsRealtime(1);
                        trafficLight.greenLight = true;
                        trafficLight.amberLight = false;
                        yield return new WaitForSecondsRealtime(intervalTime);
                        trafficLight.amberLight = true;
                        trafficLight.greenLight = false;
                        yield return new WaitForSecondsRealtime(1);
                        trafficLight.redLight = true;
                        trafficLight.amberLight = false;
                    }
                }
            }


            while (true)
            {
                foreach (TrafficLight trafficLight in trafficLights)
                {
                    trafficLight.amberLight = true;
                    trafficLight.redLight = false;
                    yield return new WaitForSecondsRealtime(1);
                    trafficLight.greenLight = true;
                    trafficLight.amberLight = false;
                    yield return new WaitForSecondsRealtime(intervalTime);
                    trafficLight.amberLight = true;
                    trafficLight.greenLight = false;
                    yield return new WaitForSecondsRealtime(1);
                    trafficLight.redLight = true;
                    trafficLight.amberLight = false;
                }
            }
        }
    }
}