using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Temporary script to make the background music frok main menu persist to tutorial menu
/// </summary>
public class PersistanceBehaviour : MonoBehaviour
{
    
    private void Start()
    {
        HandleBackgroundMusic();
    }

    public void HandleBackgroundMusic()
    {
        Scene scene = SceneManager.GetActiveScene();
        Debug.Log(scene.ToString());

        //If there are multiple copies of background music, then destroy them
        GameObject[] musicObj = GameObject.FindGameObjectsWithTag("Music");
        if (musicObj.Length > 1)
        {
            Destroy(this.gameObject);
        }
        //For main level, destroy since we do not need it
        if (scene.name == "MainLevel")
        {
            Destroy(this.gameObject);
        }


        DontDestroyOnLoad(this.gameObject);
    }


}
