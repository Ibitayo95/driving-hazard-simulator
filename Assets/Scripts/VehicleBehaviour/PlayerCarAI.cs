using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCarAI : MonoBehaviour
{
    public LayerMask trafficLayer;
    public float obstacleDetectionDistance = 5f;
    public Vector3 frontSensorPosition = new(0, 1f, 2.5f); // may need to adjust based on car length
    public float frontSensorWidth = 1.5f; // adjust based on car width

    // Start is called before the first frame update
    void Start()
    {
    }


    public bool DetectObstacle(CarController car)
    {
        bool obstacleDetected = false;
        RaycastHit hit;
        // Compute sensor position in world space
        Vector3 sensorStartPos = transform.TransformPoint(frontSensorPosition);

        // front centre sensor
        if (Physics.Raycast(sensorStartPos, transform.forward, out hit, obstacleDetectionDistance, trafficLayer))
        {
            Debug.DrawLine(sensorStartPos, hit.point);
            // brake here
            car.ApplyHandBrake();
            obstacleDetected = true;
        }

        // front left sensor
        else if (Physics.Raycast(sensorStartPos - transform.right * frontSensorWidth, transform.forward, out hit, obstacleDetectionDistance, trafficLayer))
        {
            Debug.DrawLine(sensorStartPos - transform.right * frontSensorWidth, hit.point);
            // brake here
            car.ApplyHandBrake();
            obstacleDetected = true;
        }

        // front right sensor
        else if (Physics.Raycast(sensorStartPos + transform.right * frontSensorWidth, transform.forward, out hit, obstacleDetectionDistance, trafficLayer))
        {
            Debug.DrawLine(sensorStartPos + transform.right * frontSensorWidth, hit.point);
            // brake here
            car.ApplyHandBrake();
            obstacleDetected = true;
        }

        else
        {
            car.ReleaseBrake();
        }
        return obstacleDetected;
    }
}
