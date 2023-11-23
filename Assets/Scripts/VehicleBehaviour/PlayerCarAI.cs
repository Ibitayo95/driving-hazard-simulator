using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCarAI : MonoBehaviour
{
    public WheelController car;
    public LayerMask trafficLayer;
    public float obstacleDetectionDistance = 10f;
    public Vector3 frontSensorPosition = new(0, 0.2f, 0.5f); // may need to adjust based on car length

    // Start is called before the first frame update
    void Start()
    {
        car = GetComponent<WheelController>();
    }

    private void FixedUpdate()
    {
        DetectObstacle();
    }


    void DetectObstacle()
    {
        RaycastHit hit;
        Vector3 sensorStartPos = transform.position;
        sensorStartPos += transform.forward * frontSensorPosition.z;
        sensorStartPos += transform.up * frontSensorPosition.y;
       
        // front centre sensor
        if (Physics.Raycast(sensorStartPos, transform.forward, out hit, obstacleDetectionDistance, trafficLayer))
        {
            Debug.DrawLine(sensorStartPos, hit.point);
            // brake here
            car.ApplyBrake();
        }
        else
        {
            car.ReleaseBrake();
        }
    }
}
