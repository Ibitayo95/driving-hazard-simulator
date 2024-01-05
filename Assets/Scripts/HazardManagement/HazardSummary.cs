using HazardManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class HazardSummary : MonoBehaviour
{
    private HazardManager hazardManager;
    public AverageScoreSummary averageScore; // set this one in the editor by dragging the correct component
    public HazardDto[] hazards;
    public TMP_Text[] descriptions;
    public TMP_Text[] responseTimes;


    void Start()
    {
        hazardManager = HazardManager.GetInstance();
        hazards = hazardManager.GetHazards();
        UpdateHazardSummaries();
        averageScore.UpdateScoreSummary(hazards);
    }



    private void UpdateHazardSummaries()
    {
        GameObject[] hazardSummaries = FindObsWithTag("HazardSummary"); // should return a list of 5 objects in order

        if (hazards.Length > descriptions.Length) // edge case where we have more than 5 hazards
        {
            Array.Resize(ref hazards, descriptions.Length);
        }
        
        for (int i = 0; i < hazards.Length; i++)
        {
            GameObject currentHazard = hazardSummaries[i];
            Light light = currentHazard.GetComponentInChildren<Light>();

            string hazardDescription = hazards[i].Description;
            float hazardResponseTime = hazards[i].ReactionTime;
            string hazardResponseTimeText;

            if (hazardResponseTime != -1)
            {
                light.color = Color.green;
                hazardResponseTimeText = $"Response time: {hazardResponseTime:F2} seconds";
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
