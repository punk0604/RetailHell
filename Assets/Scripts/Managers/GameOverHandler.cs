using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverHandler : MonoBehaviour
{
    [Header("Scene to Load")]
    public string gameOverScene = "GameOver"; 
    public void LoadGameOverScene()
    {
        Cursor.visible = true; // Show cursor
        Cursor.lockState = CursorLockMode.None; // Free the cursor
        Debug.Log("Loading Game Over scene...");
        SceneManager.LoadScene(gameOverScene);
    }

     public void HandleGameOver()
    {
        GameManager.Instance.SaveResumeStats();
        GameManager.Instance.ResetShifts();
        GameManager.Instance.ResetPaycheck();
    }
}
