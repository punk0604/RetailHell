using UnityEngine;
using UnityEngine.SceneManagement;

public class BreakroomDoorInteraction : MonoBehaviour
{
    private bool playerInRange = false;

    [Header("UI Panel for Door")]
    public GameObject exitUI;

    [Header("Scene Settings")]
    public string retailStore;

    [Header("Camera Control")]
    public MonoBehaviour cameraControlScript; // e.g. PlayerLook or FirstPersonLook

    private void Start()
    {
        if (exitUI != null)
            exitUI.SetActive(false);
    }

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            ToggleExitUI();
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
            CloseExitUI();
            Debug.Log("Player left breakroom door range.");
        }
    }

    // 🔁 Toggles the exit UI manually
    private void ToggleExitUI()
    {
        if (exitUI == null) return;

        bool shouldShow = !exitUI.activeSelf;
        exitUI.SetActive(shouldShow);
        gameObject.GetComponent<AudioSource>().Play();

        Cursor.lockState = shouldShow ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = shouldShow;

        if (cameraControlScript != null)
            cameraControlScript.enabled = !shouldShow;

        Debug.Log($"Breakroom Door: UI {(shouldShow ? "opened" : "closed")}.");
    }

    // ✅ Button-compatible method
    public void LoadRetailStoreScene()
    {
        if (!string.IsNullOrEmpty(retailStore))
        {
            GameObject.Find("UI button sound").GetComponent<AudioSource>().Play();
            Debug.Log($"Loading scene: {retailStore}");
            SceneManager.LoadScene(retailStore);
        }
        else
        {
            Debug.LogWarning("No scene name provided for BreakroomDoorInteraction.");
        }
    }

    // ✅ Button-compatible method
    public void CloseExitUI()
    {
        if (exitUI != null)
        {
            exitUI.SetActive(false);
            

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            if (cameraControlScript != null)
                cameraControlScript.enabled = true;

            Debug.Log("Exit UI closed.");
        }
    }
}


