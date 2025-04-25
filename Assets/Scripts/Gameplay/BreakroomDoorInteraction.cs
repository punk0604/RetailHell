using UnityEngine;

public class BreakroomDoorInteraction : MonoBehaviour
{
    private bool playerInRange = false;

    [Header("UI Panel for Door")]
    public GameObject BRdoorUI;

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (BRdoorUI != null)
            {
                bool isActive = BRdoorUI.activeSelf;
                BRdoorUI.SetActive(!isActive);
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
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;

            if (BRdoorUI != null)
            {
                BRdoorUI.SetActive(false); // Optional: hide UI when leaving
            }

            Debug.Log("Player left door range.");
        }
    }
}