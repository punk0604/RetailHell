using UnityEngine;

public class PauseMenuHandler : MonoBehaviour
{
    public static PauseMenuHandler Instance;
    public GameObject pausePanel;
    public PlayerMovement playerMovement;
    private bool isPaused = false;
    public GameObject MainCanvas;

    /*private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        //else Destroy(gameObject);
      
    }*/

    private void Awake()
    {
        if (pausePanel == null)
            pausePanel = GameObject.Find("PausePanel");
    }

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

        MainCanvas.SetActive(false);

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

        MainCanvas.SetActive(true);

        if (playerMovement != null)
            playerMovement.inputLocked = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

}
