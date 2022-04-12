using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// This script handles functionality to activate or deactivate the pause menu
/// </summary>
public class PauseMenu : MonoBehaviour
{
    private bool isPaused;
    [SerializeField] private GameObject targetMenu;
    
    void Start()
    {
        isPaused = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Pause(InputAction.CallbackContext context)
    {
        if (isPaused)
        {
            targetMenu.SetActive(false);
            Time.timeScale = 1.0f;
            Cursor.visible = false;
            isPaused = false;
            Debug.Log("Pause menu inactive");
        }
        else
        {
            targetMenu.SetActive(true);
            Cursor.visible = true;
            Time.timeScale = 0.0f;
            isPaused = true;
            Debug.Log("Pause menu active");
        }
    }

    /// <summary>
    /// This is used by the resume button of the pause menu
    /// </summary>
    public void ResumeGame()
    {
        targetMenu.SetActive(false);
        Time.timeScale = 1.0f;
        Cursor.visible = false;
        isPaused = false;
    }
}
