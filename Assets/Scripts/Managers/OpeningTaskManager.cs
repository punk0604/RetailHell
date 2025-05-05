using UnityEngine;

public class OpeningTaskManager : MonoBehaviour
{
    [Header("Opening Task Settings")]
    public int totalRegisters = 3; // Total number of registers to open
    private int openedRegisters = 0;

    public GameObject openingTasks; // UI panel for opening tasks
    public Collider[] registerColliders; // ✅ Assign colliders used for opening registers

    public ShiftSystem shiftSystem; // ✅ Reference to shift phase

    private void Update()
    {
        // ✅ Disable colliders if we have entered the Active phase
        if (shiftSystem != null && shiftSystem.currentPhase == ShiftSystem.ShiftPhase.Active)
        {
            foreach (var col in registerColliders)
            {
                if (col != null && col.enabled)
                    col.enabled = false;
            }
        }
    }

    public void RegisterOpened()
    {
        openedRegisters++;
        Debug.Log($"Registers opened: {openedRegisters}/{totalRegisters}");

        if (openedRegisters >= totalRegisters)
        {
            Debug.Log("✅ All registers opened! Opening tasks complete.");
            if (openingTasks != null)
                openingTasks.SetActive(false);
        }
    }
}

