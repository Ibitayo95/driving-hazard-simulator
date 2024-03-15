using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class HazardManager : MonoBehaviour
{
    public ActionBasedController LeftController;
    public ActionBasedController RightController;
    public static HazardManager Instance { get; private set; }
    public AudioSource reactionSound;

    // This queue stores the reaction time for each hazard. Gets emptied by GetHazards()
    private readonly Queue<HazardDto> _hazards = new();
    public bool isSummarySceneLoading = false;
    public bool HazardActivated = false;
    public int NumberOfHazardsOccurred = 0; 


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

    private void Update()
    {
        ControllerValidation();
        if (NumberOfHazardsOccurred > 0 && 
            NumberOfHazardsOccurred % 5 == 0 && 
            !isSummarySceneLoading)
        {
            StartCoroutine(LoadSummaryAfterDelay());
        }
    }
    
    public static HazardManager GetInstance()
    {
        return Instance;
    }
    
    private IEnumerator LoadSummaryAfterDelay()
    {
        isSummarySceneLoading = true;
        yield return StartCoroutine(Delay());
        SceneManager.LoadScene("Summary");
    }
    

    public void ActivateHazard(IHazardObject hazard)
    {
        // Activate the hazard 
        hazard.ActivateHazard();
        HazardActivated = true;
        Debug.Log("Hazard activated = timer starting...");
        StartCoroutine(StartReactionTimer(hazard));

    }

    public void ResolveHazard(IHazardObject hazard)
    {
        hazard.DeactivateHazard();
        HazardActivated = false;
        NumberOfHazardsOccurred++;
    }

    
    public IEnumerator Delay()
    {
        yield return new WaitForSecondsRealtime(1); // so when final hazard occurs, the transition isnt sudden   
    }

    public IEnumerator StartReactionTimer(IHazardObject hazard)
    {
        HazardDto newHazard = new HazardDto { Description = hazard.Name, ReactionTime = -1, Type = hazard.Type };
        // get the hazard's offsetTime first and wait for that number of seconds. Then start the timer 
        float offset = hazard.HazardOffsetTime;
        if (offset > 0)
        {
            yield return new WaitForSecondsRealtime(offset);
        }
        float reactionTime = 0;
        while (reactionTime < 5)
        {
            ControllerValidation();
            // space, left trigger or right trigger
            if (Input.GetKeyDown(KeyCode.Space) || IsTriggerPressed(LeftController) || IsTriggerPressed(RightController))
            {
                // Log the reaction time and end the timer
                newHazard.ReactionTime = reactionTime;
                _hazards.Enqueue(newHazard);
                if (reactionSound != null) reactionSound.Play();
                
                ResolveHazard(hazard);
                yield break;
            }
            // If the user does not react, increment the timer
            reactionTime += Time.deltaTime;
            yield return null;
        }

        // If the timer reaches 5 seconds without the user reacting, then HRT stays as default -1
        _hazards.Enqueue(newHazard);
        ResolveHazard(hazard);
    }



    // Retrieves hazards and empties the queue
    public HazardDto[] GetHazards()
    {
        HazardDto[] hazardList = new HazardDto[_hazards.Count];
        int len = _hazards.Count;
        for (int i = 0; i < len; i++)
        {
            hazardList[i] = _hazards.Dequeue();
        }

        return hazardList;
    }

    private void ControllerValidation()
    {
        if (LeftController == null)
        {
           LeftController = GameObject.FindWithTag("LeftController")?.GetComponent<ActionBasedController>();
        }
        if (RightController == null)
        {
            RightController = GameObject.FindWithTag("RightController")?.GetComponent<ActionBasedController>();
        }
    }
    
    private bool IsTriggerPressed(ActionBasedController controller)
    {
        return controller.activateAction.action.WasPressedThisFrame();
    }


}

