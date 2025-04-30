using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuController : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return)) // When ENTER is pressed
        {
            LoadMainMenu();
        }
    }

    void LoadMainMenu()
    {
        GameObject.Find("UI button sound").GetComponent<AudioSource>().Play(); //plays ui button sound
        SceneManager.LoadScene("MainMenu"); // Change scene to MainMenu
    }
}
