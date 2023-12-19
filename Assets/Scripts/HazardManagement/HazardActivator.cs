using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
	Single script that exists on the user car to activate hazards that are within the user's field of view
*/

public class HazardActivator : MonoBehaviour
{
    private HazardManager hazardManager;
    private static readonly System.Random oddsGenerator = new();
    private int hazardHuman;
    private int hazardCar;
    // set in the editor
    public float minHazardOffsetTime = 0f;
    public float maxHazardOffsetTime;


    void Start()
    {
        hazardManager = hazardManager.GetInstance();
        hazardHuman = LayerMask.NameToLayer("Humans");
        hazardCar = LayerMask.NameToLayer("HazardCar");
    }

    // could activate the hazard - depends on the chance of it occuring
    private void OnTriggerEnter(Collider other)
    {
        int objectLayer = other.gameObject.layer;

        if (objectLayer == hazardHuman || objectLayer == hazardCar) // make sure we've triggered a hazard
        {
            // get hazard object
            IHazardObject hazard = other.GetComponent<IHazardObject>();
            if (hazard == null)
            {
                Debug.LogError("No hazard component detected on GameObject");
                return;
            }
            // ignore if its outside the range of the attached trigger zone (e.g. 4-7 seconds)
            if (!(hazard.HazardOffsetTime >= minHazardOffsetTime && hazard.HazardOffsetTime < maxHazardOffsetTime))
            {
                Debug.Log($"Hazard skipped because outside of trigger zone range: {hazard.Name}");
                return;
            }

            // use odds to maybe activate hazard
            int hazardOccurance = hazard.ChanceOfOccuring; // should be between 0 - 100
            int chance = oddsGenerator.Next(0, 100);

            if (hazardOccurance > chance)
            {
                hazardManager.ActivateHazard(hazard);
            }
        }

    }
}
