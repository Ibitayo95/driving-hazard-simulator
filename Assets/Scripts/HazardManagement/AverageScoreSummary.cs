using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AverageScoreSummary : MonoBehaviour
{
    private HazardManager hazardManager;
    public TMP_Text percentageSummary;
    public TMP_Text averageResponseSummary;

    // Start is called before the first frame update
    void Start()
    {
        hazardManager = hazardManager.GetInstance();
        UpdateScoreSummary();
        
    }

    private void UpdateScoreSummary()
    {
        float accuracyScore = hazardManager.GetAccuracyScore();
        percentageSummary.SetText($"Percentage hazards spotted: {accuracyScore}%");

        float responseAverage = hazardManager.GetAverageResponseTime();
        if (responseAverage != -1)
        {
            averageResponseSummary.SetText($"Average response time: {responseAverage}");
        }
        else
        {
            averageResponseSummary.SetText(string.Empty);
        }
        
    }


}
