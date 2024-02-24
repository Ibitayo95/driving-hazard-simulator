using UnityEngine;

namespace Traffic
{
    public class TrafficLight : MonoBehaviour
    {
        public bool redLight;
        public bool amberLight;
        public bool greenLight;

        public Light red;
        public Light amber;
        public Light green;
        private void Update()
        {
            
            if (redLight)
            {
                red.enabled = true;
                amber.enabled = false;
                green.enabled = false;
            }
            else if (amberLight)
            {
                amber.enabled = true;
                red.enabled = false;
                green.enabled = false;
            }
            else if (greenLight)
            {
                green.enabled = true;
                red.enabled = false;
                amber.enabled = false;
            }
        }
    }
}