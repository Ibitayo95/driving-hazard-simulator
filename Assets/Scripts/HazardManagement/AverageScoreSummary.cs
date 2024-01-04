using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AverageScoreSummary : MonoBehaviour
{
    public TMP_Text percentageSummary;
    public TMP_Text averageResponseSummary;

 
    public void UpdateScoreSummary(HazardDto[] hazards)
    {
        float accuracyScore = GetAccuracyScore(hazards);
        percentageSummary.SetText($"Percentage hazards spotted: {accuracyScore:F1}%");

        float responseAverage = GetAverageResponseTime(hazards);
        if (responseAverage != -1)
        {
            averageResponseSummary.SetText($"Average response time: {responseAverage:F2}");
        }
        else
        {
            averageResponseSummary.SetText("No average response time available");
        }
        
    }

    private float GetAccuracyScore(HazardDto[] hazards)
    {
        float numHazards = _hazards.Count;
        if (numHazards == 0) return 0;

        float numCorrectlyIdentified = 0;

        foreach (HazardDto hz in hazards)
        {
            if (hz.ReactionTime != -1)
            {
                numCorrectlyIdentified++;
            }
        }

        return (numCorrectlyIdentified / numHazards) * 100;
    }

    private float GetAverageResponseTime(HazardDto[] hazards)
    {
        float totalResponseTime = 0;
        float numCorrectlyIdentified = 0;
        foreach (HazardDto hz in hazards)
        {
            if (hz.ReactionTime != -1)
            {
                totalResponseTime += hz.ReactionTime;
                numCorrectlyIdentified++;
            }
        }

        return (numCorrectlyIdentified == 0) ? -1 : (totalResponseTime / numCorrectlyIdentified);
    }


}
