using UnityEngine;

public class VendingMachineInteraction : MonoBehaviour
{
    private bool playerInRange = false;
    private bool isUIShown = false;

    [Header("UI Panel for Shop")]
    public GameObject shopUI;

    [Header("Player Movement Script")]
    public PlayerMovement playerMovement; // Assign your PlayerMovement script here

    public GameObject beepSound;

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (shopUI != null && playerMovement != null)
            {
                isUIShown = !isUIShown; // Toggle the UI state
                shopUI.SetActive(isUIShown);
                playerMovement.inputLocked = isUIShown; // Set inputLocked based on UI state
                beepSound.GetComponent<AudioSource>().Play();

                Debug.Log("Vending Machine: Shop UI toggled. Input Locked: " + isUIShown);

                if (isUIShown)
                {
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                }
                else
                {
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                }
            }
            else
            {
                if (shopUI == null)
                {
                    Debug.LogWarning("Vending Machine: Shop UI not assigned.");
                }
                if (playerMovement == null)
                {
                    Debug.LogWarning("Vending Machine: PlayerMovement script not assigned.");
                }
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
            Debug.Log("Paycheck: " + GameManager.Instance.GetPaycheck());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;

            if (shopUI != null)
            {
                shopUI.SetActive(false);
                isUIShown = false; // Ensure UI state is reset on exit
                if (playerMovement != null)
                {
                    playerMovement.inputLocked = false; // Unlock input when leaving range
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                }
            }

            Debug.Log("Player left vending machine range.");
            InteractionPromptUI.Instance?.Hide();
        }
    }
}
