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
    public Dictionary<HazardType, GameObject> hazardMappings = new();
    // To set up in the editor
    public HazardType[] hazardTypes;
    public GameObject[] hazardObjects; // the order of this array must match up with the types
  
    void Start()
    {
        // Fill the dictionary using the smallest length of both arrays (they should be the same though)
        int len = hazardTypes.Length <= hazardObjects.Length ? hazardTypes.Length : hazardObjects.Length;
        for (int i = 0; i < len; i++)
        {
            hazardMappings[hazardTypes[i]] = hazardObjects[i];
        }
        hazardManager = HazardManager.GetInstance();
        _hazards = hazardManager.GetHazards();
        UpdateHazardSummaries();
    }


    private void UpdateHazardSummaries()
    {
        GameObject[] hazardSummaries = FindObsWithTag("HazardSummary"); // should return a list of 5 objects in order

        if (_hazards.Length > descriptions.Length) // edge case where we have more than 5 hazards
        {
            Array.Resize(ref _hazards, descriptions.Length);
        }
        
        for (int i = 0; i < _hazards.Length; i++)
        {
            GameObject currentHazard = hazardSummaries[i];
            Light light = currentHazard.GetComponentInChildren<Light>();

            string hazardDescription = _hazards[i].Description;
            float hazardResponseTime = _hazards[i].ReactionTime;
            HazardType type = _hazards[i].Type;

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
            // TODO: instantiate the hazard prefab by retrieving it from dictionary
            GameObject prefab = Instantiate(hazardMappings[type], currentHazard.transform.position, Quaternion.identity); 
            // TODO: then re-size/rotate appropriately
            prefab.transform.localScale += new Vector3(-0.5f, -0.5f, -0.5f);
            
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
