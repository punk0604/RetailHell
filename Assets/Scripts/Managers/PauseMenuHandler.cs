using UnityEngine;

public class PauseMenuHandler : MonoBehaviour
{
    public GameObject pausePanel;
    public PlayerMovement playerMovement;
    private bool isPaused = false;

    private void Start()
    {
        if (pausePanel != null)
            pausePanel.SetActive(false);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        pausePanel.SetActive(isPaused);

        if (playerMovement != null)
            playerMovement.inputLocked = isPaused;

        Time.timeScale = isPaused ? 0 : 1;
        Cursor.visible = isPaused;
        Cursor.lockState = isPaused ? CursorLockMode.None : CursorLockMode.Locked;
    }

    public void ResumeGame()
    {
        isPaused = false;
        pausePanel.SetActive(false);
        Time.timeScale = 1;

        if (playerMovement != null)
            playerMovement.inputLocked = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

}
