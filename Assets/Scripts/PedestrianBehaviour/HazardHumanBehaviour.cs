using System.Collections;
using System.Collections.Generic;
using UnityEngine.Serialization;
using UnityEngine;


public class HazardHumanBehaviour : MonoBehaviour, IHazardObject
{
    // Hazard identifiers
    [FormerlySerializedAs("Name")][SerializeField] private string _name;
    public string Name => _name;

    [FormerlySerializedAs("ChanceOfOccuring")][SerializeField] private int _chanceOfOccuring;
    public int ChanceOfOccuring => _chanceOfOccuring;

    [FormerlySerializedAs("hazardOffsetTime")][SerializeField] private float _hazardOffsetTime;
    public float HazardOffsetTime => _hazardOffsetTime;

    [SerializeField] private HazardType _hazardType;
    public HazardType Type => _hazardType;

    private Animator animator;
    private bool hazardActivated = false;
    private bool setAnimation = false;
    private float rotationSpeed = 5.0f;
    private int currentWP = 0;

    // tinker with these in the editor
    public Transform[] waypoints;
    public bool isRunning = false;
    public float walkingSpeed = 2.0f;
    public float runningSpeed = 4.0f;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (!hazardActivated) return;
        if (currentWP >= waypoints.Length) // check if we have reached the destination
        {
            hazardActivated = false;
            DeactivateAnimations();
            return;
        }
        else
        {
            // ensure animator is either running or walking
            ActivateAnimations();
            MoveToNextWaypoint();
        }
        
    }

    [ContextMenu("Activate")]
    public void ActivateHazard()
    {
        currentWP = 0;
        hazardActivated = true;
    }

    [ContextMenu("Deactivate")]
    public void DeactivateHazard()
    {
        hazardActivated = false;
        Destroy(gameObject, 3.0f);
    }

    private void MoveToNextWaypoint()
    {
        
        // move to next waypoint
        if (Vector3.Distance(transform.position, waypoints[currentWP].position) < 1f)
        {
            currentWP++;
        }
        if (currentWP >= waypoints.Length) return;

        
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

    private void DeactivateAnimations()
    {
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

    private void ActivateAnimations()
    {
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
    }

    // to visualise the pedestrian's waypoints/route
    private void OnDrawGizmos()
    {
        if (waypoints == null || waypoints.Length == 0) return;
        foreach (Transform t in waypoints)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(t.position, 0.5f);
        }

        Gizmos.color = Color.yellow;
        // lines that connect the route of waypoints
        for (int i = 0; i < waypoints.Length - 1; i++)
        {
            Gizmos.DrawLine(waypoints[i].position, waypoints[i + 1].position);
        }
    }
}

