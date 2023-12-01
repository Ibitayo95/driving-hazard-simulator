using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCarAI : MonoBehaviour
{
    public LayerMask trafficLayer;
    public LayerMask obstacleLayer;
    public LayerMask[] layers; 
    public float obstacleDetectionDistance = 5f;
    public Vector3 frontSensorPosition = new(0, 1f, 2.5f); // may need to adjust based on car length
    public float frontSensorWidth = 1.5f; // adjust based on car width
    public float frontBackupSensorWidth = 0.68f;

    // Start is called before the first frame update
    void Start()
    {
        layers = new LayerMask[] { trafficLayer, obstacleLayer };
    }


    public bool DetectObstacle(CarController car)
    {
        RaycastHit hit;
        // Compute sensor position in world space
        Vector3 sensorStartPos = transform.TransformPoint(frontSensorPosition);

        foreach (LayerMask layer in layers)
        {
            // front centre sensor
            if (Physics.Raycast(sensorStartPos, transform.forward, out hit, obstacleDetectionDistance, layer))
            {
       
                Debug.DrawLine(sensorStartPos, hit.point);
                // brake here
                car.ApplyHandBrake();
                return true;
            }

            // front left sensor
            else if (Physics.Raycast(sensorStartPos - transform.right * frontSensorWidth, transform.forward, out hit, obstacleDetectionDistance, layer))
            {
                Debug.DrawLine(sensorStartPos - transform.right * frontSensorWidth, hit.point);
                // brake here
                car.ApplyHandBrake();
                return true;
            }

            // front right sensor
            else if (Physics.Raycast(sensorStartPos + transform.right * frontSensorWidth, transform.forward, out hit, obstacleDetectionDistance, layer))
            {
                Debug.DrawLine(sensorStartPos + transform.right * frontSensorWidth, hit.point);
                // brake here
                car.ApplyHandBrake();
                 return true;
            }
            // front left sensor
            else if (Physics.Raycast(sensorStartPos - transform.right * frontBackupSensorWidth, transform.forward, out hit, obstacleDetectionDistance, layer))
            {
                Debug.DrawLine(sensorStartPos - transform.right * frontBackupSensorWidth, hit.point);
                // brake here
                car.ApplyHandBrake();
                return true;
            }

            // front right sensor
            else if (Physics.Raycast(sensorStartPos + transform.right * frontBackupSensorWidth, transform.forward, out hit, obstacleDetectionDistance, layer))
            {
                Debug.DrawLine(sensorStartPos + transform.right * frontBackupSensorWidth, hit.point);
                // brake here
                car.ApplyHandBrake();
                return true;
            }


            else
            {
                car.ReleaseBrake();
            }
            
        }
        return false;
    }
}
