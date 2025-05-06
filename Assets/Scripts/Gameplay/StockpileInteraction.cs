using UnityEngine;

public class StockpileInteraction : MonoBehaviour
{
    private bool playerInRange = false;
    private bool stockpileAccessed = false;

    [Header("References")]
    public ShelfManager shelfManager;
    public ShiftSystem shiftSystem;

    private void Awake()
    {
        // Attempt to assign ShelfManager if not already assigned
        if (shelfManager == null)
        {
            shelfManager = FindObjectOfType<ShelfManager>();
            if (shelfManager != null)
                Debug.Log("✅ StockpileInteraction: ShelfManager assigned via FindObjectOfType.");
            else
                Debug.LogWarning("⚠️ StockpileInteraction: ShelfManager NOT found!");
        }

        // Attempt to assign ShiftSystem if not already assigned
        if (shiftSystem == null)
        {
            shiftSystem = FindObjectOfType<ShiftSystem>();
            if (shiftSystem != null)
                Debug.Log("✅ StockpileInteraction: ShiftSystem assigned via FindObjectOfType.");
            else
                Debug.LogWarning("⚠️ StockpileInteraction: ShiftSystem NOT found!");
        }
    }

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (shiftSystem.currentPhase == ShiftSystem.ShiftPhase.Closing)
            {
                shelfManager.EnableRestocking(); // ✅ Can do this every time
                GameObject.Find("BoxSound").GetComponent<AudioSource>().Play(); // plays box sound
                Debug.Log("✅ Stockpile: Restocking enabled.");
            }
            else
            {
                GameObject.Find("ErrorSound").GetComponent<AudioSource>().Play();
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


