using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    //public static PauseMenu instance;

    public GameObject pausePanel;
    public GameObject options;
    OptionsUI optionsUI;

    /*private void Awake() //make sure the sound can play across scenes
    {
        if (instance != null)
        {
            //Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }*/

    private void Start()
    {
        optionsUI = options.GetComponent<OptionsUI>();
    }


    public void Pause()
    {
        pausePanel.SetActive(true);
        Time.timeScale = 0;
    }

    public void ShowOptions()
    {
        optionsUI.ShowOptions();
    }
    public void showPause()
    {
        pausePanel.SetActive(true);
    }
    public void hidePause()
    {
        pausePanel.SetActive(false);
    }
    public void QuitMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }
}
