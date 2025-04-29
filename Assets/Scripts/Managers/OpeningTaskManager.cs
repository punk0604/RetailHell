using UnityEngine;

public class OpeningTaskManager : MonoBehaviour
{
    [Header("Opening Task Settings")]
    public int totalRegisters = 3; // How many registers you have
    private int openedRegisters = 0;

    public GameObject openingTasks; // The opening task UI or object

    public void RegisterOpened()
    {
        openedRegisters++;

        Debug.Log($"Registers opened: {openedRegisters}/{totalRegisters}");

        if (openedRegisters >= totalRegisters)
        {
            Debug.Log("✅ All registers opened! Opening tasks complete.");
            openingTasks.SetActive(false); // Disable opening tasks UI
            // You can trigger the next part of the shift here if you want
        }
    }
}
