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
            Debug.Log("Player pressed E near the stockpile.");

            if (shiftSystem.currentPhase == ShiftSystem.ShiftPhase.Closing)
            {
                if (!stockpileAccessed)
                {
                    stockpileAccessed = true;
                    shelfManager.EnableRestocking();
                    Debug.Log("✅ Stockpile: Player accessed stockpile. Restocking now enabled.");
                }
                else
                {
                    Debug.LogWarning("⚠️ Stockpile: Player already accessed stockpile. No need to do it again.");
                }
            }
            else
            {
                Debug.LogWarning($"🚫 Stockpile: Cannot access stockpile right now! Current phase is: {shiftSystem.currentPhase}");
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            Debug.Log("Stockpile: Player ENTERED stockpile area.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            Debug.Log("Stockpile: Player EXITED stockpile area.");
        }
    }
}


