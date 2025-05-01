using UnityEngine;

public class StockpileInteraction : MonoBehaviour
{
    private bool playerInRange = false;
    private bool stockpileAccessed = false;

    [Header("References")]
    public ShelfManager shelfManager;
    public ShiftSystem shiftSystem;

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (shiftSystem.currentPhase == ShiftSystem.ShiftPhase.Closing)
            {
                shelfManager.EnableRestocking(); // ✅ Can do this every time
                Debug.Log("✅ Stockpile: Restocking enabled.");
            }
            else
            {
                Debug.LogWarning($"🚫 Stockpile: Cannot access during {shiftSystem.currentPhase}.");
            }
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            Debug.Log("Stockpile: Player ENTERED stockpile area.");
            InteractionPromptUI.Instance?.Show("Press E to Grab Items");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            Debug.Log("Stockpile: Player EXITED stockpile area.");
            InteractionPromptUI.Instance?.Hide();
        }
    }
}


