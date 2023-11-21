using GleyTrafficSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRoute : MonoBehaviour
{
    public static PlayerRoute[] route;

    private void Awake()
    {
        route = FindObjectsOfType<PlayerRoute>();
    }
}
