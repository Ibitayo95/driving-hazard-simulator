using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MenuTasks : MonoBehaviour
{
    public TMP_Text loadingProgressText; // Reference to the UI Text element

    public void LoadMainSimulation(bool carPositionRandomised)
    {
        SimulationConfig.CarPositionRandomised = carPositionRandomised;
        string sceneName = SimulationConfig.IsHighFidelity ? "MainSimulationHighFidelity" : "MainSimulation";
        StartCoroutine(LoadMainSimulationAsync(sceneName));

    }

    IEnumerator LoadMainSimulationAsync(string sceneName)
    {
        AsyncOperation asyncLoadOperation = SceneManager.LoadSceneAsync(sceneName);
        while (!asyncLoadOperation.isDone)
        {
            // whilst waiting we update the loading progress
            float progress = Mathf.Clamp01(asyncLoadOperation.progress / 0.9f);
            loadingProgressText.text = "Loading: " + (progress * 100).ToString("F0") + "%";
            yield return null;
        }
    }


    public void ExitMainMenu()
    {
        Application.Quit();
    }

    public void ToggleHighFidelityMode()
    {
        SimulationConfig.IsHighFidelity = !SimulationConfig.IsHighFidelity;
    }


}