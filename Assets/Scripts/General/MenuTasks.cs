using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MenuTasks : MonoBehaviour
{
    private AsyncOperation asyncLoadOperation;
    public TMP_Text loadingProgressText; // Reference to the UI Text element

    public void LoadMainSimulation()
    {
        SimulationConfig.CarPositionRandomised = false;
        string sceneName = SimulationConfig.IsHighFidelity ? "MainSimulationHighFidelity" : "MainSimulation";
        asyncLoadOperation = SceneManager.LoadSceneAsync(sceneName);
        asyncLoadOperation.allowSceneActivation = false; // Don't activate the scene immediately
        StartCoroutine(LoadingProgress());
    }

    public void RestartSimulation()
    {
        SimulationConfig.CarPositionRandomised = true;
        string sceneName = SimulationConfig.IsHighFidelity ? "MainSimulationHighFidelity" : "MainSimulation";
        asyncLoadOperation = SceneManager.LoadSceneAsync(sceneName);
        asyncLoadOperation.allowSceneActivation = false; // Don't activate the scene immediately
        StartCoroutine(LoadingProgress());
    }

    public void ExitMainMenu()
    {
        Application.Quit();
    }

    public void ToggleHighFidelityMode()
    {
        SimulationConfig.IsHighFidelity = !SimulationConfig.IsHighFidelity;
    }

    private void Update()
    {
        if (asyncLoadOperation != null && asyncLoadOperation.isDone)
        {
            // Activate the loaded scene when it's ready
            asyncLoadOperation.allowSceneActivation = true;
            asyncLoadOperation = null; // Reset the operation
        }
    }
    
    private IEnumerator LoadingProgress()
    {
        while (!asyncLoadOperation.isDone)
        {
            float progress = Mathf.Clamp01(asyncLoadOperation.progress / 0.9f);
            loadingProgressText.text = "Loading: " + (progress * 100).ToString("F0") + "%";
            yield return null;
        }
    }
}