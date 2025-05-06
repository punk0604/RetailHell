using UnityEngine;

public class ZoneTeleporter : MonoBehaviour
{
    [Header("Settings")]
    public Transform destinationPoint;    // Where the player should be moved
    public string interactionMessage = "Press E to Teleport"; // Prompt message
    public ShiftSystem shiftSystem;

    private bool playerInRange = false;

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (shiftSystem != null &&
                (shiftSystem.currentPhase == ShiftSystem.ShiftPhase.Closing ||
                 shiftSystem.currentPhase == ShiftSystem.ShiftPhase.Break))
            {
                TeleportPlayer();
            }
            else if (shiftSystem != null)
            {
                Debug.Log($"❌ You can't teleport during the {shiftSystem.currentPhase} phase.");
            }
            else
            {
                Debug.LogWarning("⚠️ ShiftSystem reference is not set on the ZoneTeleporter.");
            }
        }
    }

    void TeleportPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null && destinationPoint != null)
        {
            CharacterController controller = player.GetComponent<CharacterController>();
            if (controller != null)
                controller.enabled = false; // Disable to avoid teleport bug with CharacterController

            player.transform.position = destinationPoint.position;

            if (controller != null)
                controller.enabled = true;

            Debug.Log("✅ Player teleported to new zone.");
        }
        else
        {
            Debug.LogError("🚫 Player or destinationPoint not assigned!");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            InteractionPromptUI.Instance?.Show(interactionMessage);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            InteractionPromptUI.Instance?.Hide();
        }
    }
}
