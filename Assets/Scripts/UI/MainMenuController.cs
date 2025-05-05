using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class MainMenuController : MonoBehaviour
{
    public Button[] menuButtons; // Array of menu buttons
    private int selectedIndex = 0; // Keeps track of which button is selected
    public GameObject options;
    OptionsUI optionsUI;

    void Start()
    {
        HighlightButton(); // Ensure the first button is highlighted
        optionsUI = options.GetComponent<OptionsUI>();
    }

    void Update()
    {
        // Navigate Down
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            selectedIndex = (selectedIndex + 1) % menuButtons.Length;
            HighlightButton();
        }

        // Navigate Up
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            selectedIndex = (selectedIndex - 1 + menuButtons.Length) % menuButtons.Length;
            HighlightButton();
        }

        // Select Option
        if (Input.GetKeyDown(KeyCode.Return))
        {
            menuButtons[selectedIndex].onClick.Invoke(); // Simulate a button click
        }
    }

     void HighlightButton()
    {
        // Reset all button texts to default
        foreach (Button button in menuButtons)
        {
            TextMeshProUGUI buttonText = button.GetComponentInChildren<TextMeshProUGUI>();
            buttonText.color = Color.white; // Default color
            buttonText.fontStyle = FontStyles.Normal; // Remove underline/bold
        }

        // Highlight selected button's text
        TextMeshProUGUI selectedText = menuButtons[selectedIndex].GetComponentInChildren<TextMeshProUGUI>();
        selectedText.color = Color.yellow; // Highlight color
        selectedText.fontStyle = FontStyles.Bold | FontStyles.Underline; // Bold + Underline for emphasis
    }

    public void StartGame()
    {
        PlayUiSound(); //plays ui button sound
        Debug.Log("Start Game"); // Placeholder for starting the game
        StartmenuMusicManager.instance.GetComponent<AudioSource>().Stop(); //VERY important to stop the menu music when the game starts
    }

    public void ShowInstructions()
    {
        GameObject.Find("UI button sound").GetComponent<AudioSource>().Play(); //plays ui button sound
        Debug.Log("Instructions Button Clicked - Show Instructions UI or Scene");
    }

    public void LoadOptions()
    {
        Time.timeScale = 0;
        Debug.Log("time paused");
        PlayUiSound(); //plays ui button sound
        optionsUI.ShowOptions();
        Debug.Log("Load Options Menu"); // Placeholder for loading options
    }

    public void QuitGame()
    {
        StartmenuMusicManager.instance.GetComponent<AudioSource>().Stop(); //stops music just in case quitting the app breaks the music somehow
        Application.Quit(); // Quit the application
        Debug.Log("Game Quit"); // Log for debugging purposes
    }

    public void PlayUiSound()
    {
        GameObject.Find("UI button sound").GetComponent<AudioSource>().Play();
    }
}
