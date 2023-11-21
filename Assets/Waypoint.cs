using UnityEngine;

public class Waypoint : MonoBehaviour
{
    public static Waypoint[] waypoints;

    private void Awake()
    {
        waypoints = FindObjectsOfType<Waypoint>();
    }
}
