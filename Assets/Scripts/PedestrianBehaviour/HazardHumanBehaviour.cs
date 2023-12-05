using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HazardHumanBehaviour : MonoBehaviour
{
    private Animator animator;
    private bool hazardActivated = false;
    private bool setAnimation = false;
    private float rotationSpeed = 2.0f;
    private int currentWP = 0;

    // tinker with these in the editor
    public Transform[] waypoints;
    public bool isRunning = false;
    public float walkingSpeed = 2.0f;
    public float runningSpeed = 4.0f;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (!hazardActivated) return;
        if (currentWP >= waypoints.Length) // check if we have reached the destination
        {
            hazardActivated = false;
            if (isRunning)
            {
                animator.SetBool("Running", false);
            }
            else
            {
                animator.SetBool("Walking", false);
            }
            setAnimation = false;
            return;
        }
        MoveToNextWaypoint();
    }

    public void ActivateHazard()
    {
        currentWP = 0;
        hazardActivated = true;
    }

    private void MoveToNextWaypoint()
    {
        // ensure animator is either running or walking
        if (!setAnimation)
        {
            if (isRunning)
            {
                animator.SetBool("Running", true);
            }
            else
            {
                animator.SetBool("Walking", true);
            }
            setAnimation = true;
        }

        // move to next waypoint
        if (Vector3.Distance(transform.position, waypoints[currentWP].position) < 3f)
        {
            currentWP++;
        }

        Quaternion direction = Quaternion.LookRotation(waypoints[currentWP].position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, direction, rotationSpeed * Time.deltaTime);

        if (isRunning)
        {
            transform.Translate(0, 0, runningSpeed * Time.deltaTime);
        }
        else
        {
            transform.Translate(0, 0, walkingSpeed * Time.deltaTime);
        }
    }

    // to visualise the pedestrian's waypoints/route
    private void OnDrawGizmos()
    {
        foreach (Transform t in waypoints)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(t.position, 0.5f);
        }

        Gizmos.color = Color.red;
        // lines that connect the route of waypoints
        for (int i = 0; i < waypoints.Length - 1; i++)
        {
            Gizmos.DrawLine(waypoints[i].position, waypoints[i + 1].position);
        }
    }
}

