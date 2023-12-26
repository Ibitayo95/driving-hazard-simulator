using HazardManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class HazardSummary : MonoBehaviour
{
    private HazardManager hazardManager;
    private HazardDto[] _hazards;
    public TMP_Text[] descriptions;
    public TMP_Text[] responseTimes;

    // Start is called before the first frame update
    void Start()
    {
        hazardManager = hazardManager.GetInstance();
        _hazards = hazardManager.GetHazards();
        UpdateHazardSummaries();
    }


    private void UpdateHazardSummaries()
    {
        GameObject[] hazardSummaries = FindObsWithTag("HazardSummary"); // should return a list of 5 objects in order
        
        for (int i = 0; i < _hazards.Length; i++)
        {
            GameObject currentHazard = hazardSummaries[i];
            Light light = currentHazard.GetComponentInChildren<Light>();

            string hazardDescription = _hazards[i].Description;
            float hazardResponseTime = _hazards[i].ReactionTime;
            string hazardResponseTimeText;

            if (hazardResponseTime != -1)
            {
                light.color = Color.green;
                hazardResponseTimeText = $"Response time: {hazardResponseTime}";
            }
            else
            {
                light.color = Color.red;
                hazardResponseTimeText = "Hazard Missed!";
            }

            // set description text
            descriptions[i].SetText(hazardDescription);
            // set response time text
            responseTimes[i].SetText(hazardResponseTimeText);
            
        }
    }

    private GameObject[] FindObsWithTag(string tag)
    {
        GameObject[] foundObs = GameObject.FindGameObjectsWithTag(tag);
        Array.Sort(foundObs, CompareObNames);
        return foundObs;
    }

    private int CompareObNames(GameObject x, GameObject y)
    {
        return x.name.CompareTo(y.name);
    }


}
