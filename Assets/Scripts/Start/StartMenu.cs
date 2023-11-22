using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
   
    public void LoadMainSimulation()
    {
        SceneManager.LoadScene("MainSimulation");
    }

    public void ExitMainMenu()
    {
        Application.Quit();
    }
}
