using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// This script has the unique purpose of loading a scene or exiting the game
/// Add to any button to load up a scene or exit environment
/// </summary>
public class LoadScene : MonoBehaviour
{
    public void Load_Scene(string sceneName)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneName);
    }
    public void ExitGame()
    {
        Application.Quit();
    }
}
