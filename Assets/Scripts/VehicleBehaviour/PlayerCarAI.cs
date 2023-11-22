using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCarAI : MonoBehaviour
{
    public LayerMask obstacleLayers;
    public Transform frontOfCar;
    public float obstacleDetectionDistance = 20f;
    private WheelController car;
    // Start is called before the first frame update
    void Start()
    {
        car = GetComponent<WheelController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void DetectObstacle()
    {
        RaycastHit hit;

        Vector3 sensorStartPos = frontOfCar.position;
       
        // forward sensor
        if (Physics.Raycast(sensorStartPos, transform.forward, out hit, obstacleDetectionDistance, obstacleLayers))
        {
            // brake here
        }
    }
}
