using UnityEngine;
using UnityEngine.SceneManagement;

public class BreakroomDoorInteraction : MonoBehaviour
{
    private bool playerInRange = false;

    [Header("UI Panel for Door")]
    public GameObject exitUI; // Use GameObject for flexibility in Inspector

    [Header("Scene Settings")]
    public string retailStore; // Scene to load when exiting

    private void Start()
    {
        if (exitUI != null)
            exitUI.SetActive(false); // Ensure UI starts hidden
    }

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (exitUI != null)
            {
                bool isActive = exitUI.activeSelf;
                exitUI.SetActive(!isActive);
                Debug.Log("Breakroom Door: UI toggled.");
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            InteractionPromptUI.Instance?.Show("Press E to Exit");
            Debug.Log("Player entered breakroom door range.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            InteractionPromptUI.Instance?.Hide();

            if (exitUI != null)
                exitUI.SetActive(false);

            Debug.Log("Player left breakroom door range.");
        }
    }

    
    public void LoadRetailStoreScene()
    {
        if (!string.IsNullOrEmpty(retailStore))
        {
            Debug.Log($"Loading scene: {retailStore}");
            SceneManager.LoadScene(retailStore);
        }
        else
        {
            Debug.LogWarning("No scene name provided for BreakroomDoorInteraction.");
        }
    }


    public void CloseExitUI()
    {
        if (exitUI != null)
        {
            exitUI.SetActive(false);
            Debug.Log("Exit UI closed.");
        }
    }
}

