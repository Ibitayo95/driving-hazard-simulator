using GleyTrafficSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRoute : MonoBehaviour
{
    public static PlayerRouteWaypoint[] route;

    private void Awake()
    {
        // get different waypoints of the player route in order
        route = GetComponentsInChildren<PlayerRouteWaypoint>();
    }
}
