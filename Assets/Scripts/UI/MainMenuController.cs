using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class MainMenuController : MonoBehaviour
{
    public Button[] menuButtons;
    private int selectedIndex = 0;
    public GameObject options;
    OptionsUI optionsUI;

    void Start()
    {
        HighlightButton();
        optionsUI = options.GetComponent<OptionsUI>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            selectedIndex = (selectedIndex + 1) % menuButtons.Length;
            HighlightButton();
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            selectedIndex = (selectedIndex - 1 + menuButtons.Length) % menuButtons.Length;
            HighlightButton();
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            menuButtons[selectedIndex].onClick.Invoke();
        }
    }

    void HighlightButton()
    {
        foreach (Button button in menuButtons)
        {
            TextMeshProUGUI buttonText = button.GetComponentInChildren<TextMeshProUGUI>();
            buttonText.color = Color.white;
            buttonText.fontStyle = FontStyles.Normal;
        }

        TextMeshProUGUI selectedText = menuButtons[selectedIndex].GetComponentInChildren<TextMeshProUGUI>();
        selectedText.color = Color.yellow;
        selectedText.fontStyle = FontStyles.Bold | FontStyles.Underline;
    }

    public void StartGame()
    {
        PlayUiSound();
        Debug.Log("Start Game");

        StartmenuMusicManager.instance.GetComponent<AudioSource>().Stop();

        SceneManager.LoadScene("retailStore"); // Load main scene
    }

    public void ShowInstructions()
    {
        PlayUiSound();
        Debug.Log("Instructions Button Clicked - Show Instructions UI or Scene");
        SceneManager.LoadScene("Instructions");
    }

    public void ClickToHome()
    {
        PlayUiSound();
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        PlayUiSound();
        StartmenuMusicManager.instance.GetComponent<AudioSource>().Stop();
        Debug.Log("Game Quit");

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // Stop play mode in the editor
#else
        Application.Quit(); // Quit actual build
#endif
    }

    public void PlayUiSound()
    {
        GameObject.Find("UI button sound").GetComponent<AudioSource>().Play();
    }
}

