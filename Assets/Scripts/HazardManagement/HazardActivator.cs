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
    
    private int hazardCar;
    // set in the editor
    public float minHazardOffsetTime = 0f;
    public float maxHazardOffsetTime;


    void Start()
    {
        hazardManager = HazardManager.GetInstance();
        hazardManager.isSummarySceneLoading = false;
        hazardManager.NumberOfHazardsOccurred = 0; 
        hazardCar = LayerMask.NameToLayer("HazardCar");
    }

    // could activate the hazard - depends on the chance of it occuring
    private void OnTriggerEnter(Collider other)
    {
        // ignore if hazard is already occuring (only one can happen at a time)
        if (hazardManager.HazardActivated) return;
        int objectLayer = other.gameObject.layer;
        

        if (objectLayer == hazardCar || other.gameObject.tag.Equals("HazardHuman"))
        {
            // get hazard object
            IHazardObject hazard = other.GetComponent<IHazardObject>();
            if (hazard == null)
            {
                Debug.LogError("Hazard component not detected on object", other);
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
                Debug.Log($"Hazard Triggered: {hazard.Name}");
            }
        }
        

    }
}
