using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarAI : MonoBehaviour
{
    public List<Transform> waypoints;
    public float speed = 10f;
    public float turnSpeed = 5f;
    public int currentWaypointIndex = 0;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (waypoints.Count == 0)
            return;

        Drive();
        CheckWaypointDistance();
    }

    void Drive()
    {
        // Calculate direction and rotate the car towards the waypoint
        Vector3 direction = (waypoints[currentWaypointIndex].position - transform.position).normalized;
        Quaternion toRotation = Quaternion.LookRotation(direction, Vector3.up);
        transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, turnSpeed * Time.deltaTime);

        // Drive towards the waypoint
        rb.AddForce(transform.forward * speed, ForceMode.Acceleration);
    }

    void CheckWaypointDistance()
    {
        // If we are closer than 2 units to the waypoint, proceed to the next waypoint
        if (Vector3.Distance(transform.position, waypoints[currentWaypointIndex].position) < 2f)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Count;
        }
    }
}
