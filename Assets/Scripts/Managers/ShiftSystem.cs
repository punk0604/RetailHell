using UnityEngine;
using UnityEngine.SceneManagement;

public class ShiftSystem : MonoBehaviour
{
    public enum ShiftPhase { Opening, Active, WaitingForCustomersToClear, Closing, Break }
    public ShiftPhase currentPhase;

    [Header("UI References")]
    public GameObject openingTaskUI;
    public GameObject closingTaskUI;

    [Header("Task Parents")]
    public GameObject openingTasks;
    public GameObject closingTasks;

    [Header("Systems")]
    public CustomerController customerController;  // ✅ Renamed from CustomerManager
    public ShelfManager shelfManager;

    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "Breakroom")
        {
            Debug.Log("ShiftSystem disabled in Breakroom scene.");
            enabled = false;
            return;
        }

        if (currentPhase == ShiftPhase.Break)
        {
            Debug.Log("Returning from Breakroom. Restarting shift.");
            ResetShift();
        }
        else
        {
            StartOpening();
        }
    }

    private void Update()
    {
        switch (currentPhase)
        {
            case ShiftPhase.Opening:
                if (AllTasksComplete(openingTasks))
                    CompleteOpening();
                break;

            case ShiftPhase.Active:
                if (customerController != null && customerController.IsSpawningComplete())
                    StopActivePhase();
                break;

            case ShiftPhase.WaitingForCustomersToClear:
                if (customerController != null && customerController.AllCustomersCompleted())
                    StartClosing();
                break;

            case ShiftPhase.Closing:
                if (AllTasksComplete(closingTasks))
                    CompleteClosing();
                break;
        }
    }

    void StartOpening()
    {
        currentPhase = ShiftPhase.Opening;

        if (openingTaskUI != null) openingTaskUI.SetActive(true);
        if (closingTaskUI != null) closingTaskUI.SetActive(false);

        if (customerController != null)
            customerController.gameObject.SetActive(false);

        if (shelfManager != null)
            shelfManager.StopRemovingItems();
    }

    void CompleteOpening()
    {
        if (openingTaskUI != null) openingTaskUI.SetActive(false);
        StartActiveShift();
    }

    void StartActiveShift()
    {
        currentPhase = ShiftPhase.Active;

        if (customerController != null)
        {
            customerController.gameObject.SetActive(true);
            customerController.StartSpawning();
        }

        if (shelfManager != null)
            shelfManager.StartRemovingItems();

        Debug.Log("Active shift started: Customers spawning + shelf items disappearing.");
    }

    void StopActivePhase()
    {
        if (customerController != null)
            customerController.StopSpawning();

        if (shelfManager != null)
            shelfManager.StopRemovingItems();

        currentPhase = ShiftPhase.WaitingForCustomersToClear;
        Debug.Log("ShiftSystem: Waiting for customers to clear.");
    }

    void StartClosing()
    {
        currentPhase = ShiftPhase.Closing;

        if (closingTaskUI != null) closingTaskUI.SetActive(true);

        Debug.Log("ShiftSystem: Closing tasks started.");
    }

    void CompleteClosing()
    {
        if (closingTaskUI != null) closingTaskUI.SetActive(false);

        Debug.Log("✅ Shift ended. Breakroom is now accessible.");
        currentPhase = ShiftPhase.Break;
    }

    bool AllTasksComplete(GameObject taskParent)
    {
        if (taskParent == null)
        {
            Debug.LogWarning("ShiftSystem: taskParent is null or missing.");
            return false;
        }

        ShiftTask[] tasks = taskParent.GetComponentsInChildren<ShiftTask>(true);
        foreach (ShiftTask task in tasks)
        {
            if (task.TaskPhase == currentPhase && !task.IsTaskComplete())
                return false;
        }

        return true;
    }

    void ResetShift()
    {
        Debug.Log("🔁 ShiftSystem: Resetting shift after Breakroom.");
        currentPhase = ShiftPhase.Opening;

        if (openingTaskUI != null) openingTaskUI.SetActive(false);
        if (closingTaskUI != null) closingTaskUI.SetActive(false);

        StartOpening();
    }
}






