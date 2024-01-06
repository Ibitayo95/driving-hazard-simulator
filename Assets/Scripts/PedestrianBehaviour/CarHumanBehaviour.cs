using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;


/*
 *  This script governs the behaviour of a special hazard in which a human exits a car which blocks the road
 */
public class CarHumanBehaviour : MonoBehaviour, IHazardObject
{
    // Hazard identifiers
    [FormerlySerializedAs("Name")] [SerializeField] private string _name;
    public string Name => _name;

    [FormerlySerializedAs("ChanceOfOccuring")] [SerializeField] private int _chanceOfOccuring;
    public int ChanceOfOccuring => _chanceOfOccuring;

    [FormerlySerializedAs("hazardOffsetTime")] [SerializeField] private float _hazardOffsetTime;
    public float HazardOffsetTime => _hazardOffsetTime;

    [SerializeField] private HazardType _hazardType;
    public HazardType Type => _hazardType;

    // set these in the editor
    public Animator carAnimator; // e.g. car door opens
    public Animator humanAnimator; // human gets out

    // private variables
    private bool hazardActivated = false;
    private bool setAnimation = false;
    private float rotationSpeed = 5.0f;
    private int currentWP = 0;
    private bool humanHasExitedCar = false;

    // tinker with these in the editor
    public Transform[] waypoints;
    public bool isRunning = false;
    public float walkingSpeed = 2.0f;
    public float runningSpeed = 4.0f;



    void Start()
    {
        humanAnimator.Play("idle_sitting", -1, 0f);
    }

    void Update()
    {
        if (!hazardActivated) return;
        if (currentWP >= waypoints.Length) // check if we have reached the destination
        {
            DeactivateAnimations();
            hazardActivated = false;
            setAnimation = false;
            return;
        }
        else
        {
            if (!setAnimation)
            {
                StartCoroutine(ActivateAnimations());
                setAnimation = true;
                
             
            }
            else if (humanHasExitedCar == true)
            {
                
                MoveToNextWaypoint();
            }
        }

    }

    [ContextMenu("Activate")]
    public void ActivateHazard()
    {
        currentWP = 0;
        humanHasExitedCar = false;
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
        if (Vector3.Distance(transform.position, waypoints[currentWP].position) < 0.8f)
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
        humanAnimator.SetBool("Walking", false);
    }

    private IEnumerator ActivateAnimations()
    {
        // 1. car door opens
        carAnimator.SetBool("OpenCarDoor", true);

        // 2. wait for x secs (car door is open during this time)
        yield return new WaitForSecondsRealtime(2);

        // 3. person emerges from car
        humanAnimator.SetBool("ExitCar", true);
        yield return new WaitForSecondsRealtime(10);
        humanHasExitedCar = true;

        // 4. person makes way to waypoint
        humanAnimator.SetBool("Walking", true);

    }

    // to visualise the pedestrian's waypoints/route
    private void OnDrawGizmos()
    {
        if (waypoints.Length == 0) return;
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
