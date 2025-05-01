using UnityEngine;

public class ShopDoorInteraction : MonoBehaviour
{
    private bool playerInRange = false;

    [Header("UI Panel for Door")]
    public GameObject ShopDoorUI;

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (ShopDoorUI != null)
            {
                bool isActive = ShopDoorUI.activeSelf;
                ShopDoorUI.SetActive(!isActive);
                Debug.Log("Door: UI Toggled.");
            }
            else
            {
                Debug.LogWarning("Door: UI Not Assigned.");
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            Debug.Log("Player entered door range.");
            InteractionPromptUI.Instance?.Show("Press E to Leave");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            InteractionPromptUI.Instance?.Hide();

            if (ShopDoorUI != null)
            {
                ShopDoorUI.SetActive(false); // Optional: hide UI when leaving
            }

            Debug.Log("Player left door range.");
        }
    }
}