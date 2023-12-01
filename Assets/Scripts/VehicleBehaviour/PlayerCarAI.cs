using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCarAI : MonoBehaviour
{
    public LayerMask trafficLayer;
    public LayerMask obstacleLayer;
    public LayerMask[] layers;
    public float obstacleDetectionDistance = 5f;
    public Vector3 frontSensorPosition = new Vector3(0, 1f, 2.5f);
    public float[] sensorWidths = new float[] { 1.5f, 0.68f };  // array of sensor widths

    // Start is called before the first frame update
    void Start()
    {
        layers = new LayerMask[] { trafficLayer, obstacleLayer };
    }

    public bool DetectObstacle(CarController car)
    {
        Vector3 sensorStartPos = transform.TransformPoint(frontSensorPosition);

        foreach (LayerMask layer in layers)
        {
            // Center, left, and right sensors
            foreach (float sensorWidth in sensorWidths)
            {
                // Check for obstacles with sensors at three positions: middle, left and right
                for (int i = -1; i <= 1; i++)
                {
                    Vector3 sensorPosition = sensorStartPos + i * transform.right * sensorWidth;
                    if (Physics.Raycast(sensorPosition, transform.forward, out RaycastHit hit, obstacleDetectionDistance, layer))
                    {
                        Debug.DrawLine(sensorPosition, hit.point);
                        car.ApplyHandBrake();
                        return true;
                    }
                }
            }
        }

        car.ReleaseBrake();
        return false;
    }
}