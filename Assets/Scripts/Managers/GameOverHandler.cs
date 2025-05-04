using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverHandler : MonoBehaviour
{
    [Header("Scene to Load")]
    public string gameOverScene = "GameOver"; 
    public void LoadGameOverScene()
    {
        Debug.Log("Loading Game Over scene...");
        SceneManager.LoadScene(gameOverScene);
    }
}
