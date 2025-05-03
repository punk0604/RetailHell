using UnityEngine;

public class CashRegister : MonoBehaviour, ShiftTask
{
    public ShiftSystem shiftSystem;

    private bool isOpened = false;
    private bool isClosed = false;
    private bool playerInRange = false;

    public ShiftSystem.ShiftPhase TaskPhase => shiftSystem.currentPhase;

    private void Update()
    {
        if (!playerInRange || !Input.GetKeyDown(KeyCode.E)) return;

        if (shiftSystem.currentPhase == ShiftSystem.ShiftPhase.Opening && !isOpened)
        {
            isOpened = true;
            gameObject.GetComponent<AudioSource>().Play();
            Debug.Log($"✅ Register '{gameObject.name}' opened.");
        }
        else if (shiftSystem.currentPhase == ShiftSystem.ShiftPhase.Closing && !isClosed)
        {
            isClosed = true;
            gameObject.GetComponent<AudioSource>().Play();
            Debug.Log($"✅ Register '{gameObject.name}' closed.");
        }
    }

    public bool IsTaskComplete()
    {
        return shiftSystem.currentPhase switch
        {
            ShiftSystem.ShiftPhase.Opening => isOpened,
            ShiftSystem.ShiftPhase.Closing => isClosed,
            _ => true
        };
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) playerInRange = true;
        InteractionPromptUI.Instance?.Show("Press E to Interact");
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) playerInRange = false;
        InteractionPromptUI.Instance?.Hide();
    }
}

