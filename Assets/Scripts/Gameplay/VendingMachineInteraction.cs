using UnityEngine;

public class VendingMachineInteraction : MonoBehaviour
{
    private bool playerInRange = false;

    [Header("UI Panel for Shop")]
    public GameObject shopUI;

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (shopUI != null)
            {
                bool isActive = shopUI.activeSelf;
                shopUI.SetActive(!isActive);
                Debug.Log("Vending Machine: Shop UI toggled.");
            }
            else
            {
                Debug.LogWarning("Vending Machine: Shop UI not assigned.");
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            Debug.Log("Player entered vending machine range.");
            InteractionPromptUI.Instance?.Show("Press E for Vending Machine");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;

            if (shopUI != null)
            {
                shopUI.SetActive(false); // Optional: hide UI when leaving
            }

            Debug.Log("Player left vending machine range.");
            InteractionPromptUI.Instance?.Hide();
        }
    }
}
