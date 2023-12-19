using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class HazardManager : MonoBehaviour
{
    public static HazardManager Instance { get; private set; }

    // This dictionary stores the reaction time for each hazard. 
    // If a hazard is not present in the dictionary, it means the user did not react to it.
    private readonly Dictionary<string, float> hazardReactionTimes = new();

    private void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public HazardManager GetInstance()
    {
        return Instance;
    }

    public void ActivateHazard(IHazardObject hazard)
    {
        // Activate the hazard 
        hazard.ActivateHazard();
        StartCoroutine(StartReactionTimer(hazard));

    }

    public IEnumerator StartReactionTimer(IHazardObject hazard)
    {
        // get the hazard's offsetTime first and wait for that number of seconds. Then start the timer 
        float offset = hazard.HazardOffsetTime;
        if (offset > 0)
        {
            yield return new WaitForSeconds(offset);
        }
        float reactionTime = 0;

        while (reactionTime < 5)
        {
            // Test with pressing space first - will change this to VR input later
            if (Input.GetKeyDown(KeyCode.Space))
            {
                // Log the reaction time and end the timer
                hazardReactionTimes[hazard.Name] = reactionTime;
                yield break;
            }
            // If the user does not react, increment the timer
            reactionTime += Time.deltaTime;
            yield return null;
        }

        // If the timer reaches 5 seconds without the user reacting, log that the user did not react to the hazard
        hazardReactionTimes[hazard.Name] = -1;
    }


    public float GetAccuracyScore()
    {
        float numHazards = hazardReactionTimes.Count;
        float numCorrectlyIdentified = 0;

        foreach (KeyValuePair<string, float> entry in hazardReactionTimes)
        {
            if (entry.Value != -1)
            {
                numCorrectlyIdentified++;
            }
        }

        return (numCorrectlyIdentified / numHazards) * 100;
    }
}

