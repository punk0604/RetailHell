using UnityEngine;

public class ShiftResetTrigger : MonoBehaviour
{
    [Header("References")]
    public ShiftSystem shiftSystem;
    public GameObject tasksParent;
    public GameObject openingTaskUI;
    public TaskStressManager stressManager;
    public ShiftConfirmationUI confirmationUI;

    private bool playerInRange = false;

    void Update()
    {
        if (!playerInRange || shiftSystem == null) return;

        if (shiftSystem.currentPhase == ShiftSystem.ShiftPhase.Closing && Input.GetKeyDown(KeyCode.E))
        {
            confirmationUI?.Show(this);
        }
    }

    public void ExecuteShiftReset()
    {
        Debug.Log("🔁 Restarting shift...");

        GameManager.Instance?.IncrementShift();
        shiftSystem.currentPhase = ShiftSystem.ShiftPhase.Opening;

        if (openingTaskUI != null)
            openingTaskUI.SetActive(true);

        if (tasksParent != null)
        {
            foreach (ShiftTask task in tasksParent.GetComponentsInChildren<ShiftTask>(true))
                task.ResetTask();
        }

        stressManager?.ResetStress();

        shiftSystem.SendMessage("StartOpening");
        Debug.Log("✅ Shift reset to Opening phase.");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            InteractionPromptUI.Instance?.Show("Press E to Start New Shift");
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

