using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuTasks : MonoBehaviour
{

    public void LoadMainSimulation()
    {
        SimulationConfig.CarPositionRandomised = false;
        if (SimulationConfig.IsHighFidelity)
        {
            SceneManager.LoadScene("MainSimulationHighFidelity");
        }
        else
        {
            SceneManager.LoadScene("MainSimulation");
        }
    }

    public void RestartSimulation()
    {
        // only set to random on simulation restarts
        SimulationConfig.CarPositionRandomised = true;
        if (SimulationConfig.IsHighFidelity)
        {
            SceneManager.LoadScene("MainSimulationHighFidelity");
        }
        else
        {
            SceneManager.LoadScene("MainSimulation");
        }
    }

    public void ExitMainMenu()
    {
        Application.Quit();
    }

    public void ToggleHighFidelityMode()
    {
        SimulationConfig.IsHighFidelity = SimulationConfig.IsHighFidelity == false ? SimulationConfig.IsHighFidelity = true : SimulationConfig.IsHighFidelity = false;
    }

}
