using UnityEngine;
using UnityEngine.SceneManagement;

public class ShiftSystem : MonoBehaviour
{
    public enum ShiftPhase { Opening, Active, WaitingForCustomersToClear, Closing, Break }
    public ShiftPhase currentPhase;

    [Header("UI References")]
    public GameObject openingTaskUI; // UI Panel for opening task list
    public GameObject closingTaskUI; // UI Panel for closing task list

    [Header("Task Parents")]
    public GameObject openingTasks; // Parent object containing opening task scripts
    public GameObject closingTasks; // Parent object containing closing task scripts

    [Header("Systems")]
    public CustomerManager customerManager;
    public ShelfManager shelfManager;

    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "Breakroom")
        {
            Debug.Log("ShiftSystem disabled in Breakroom scene.");
            enabled = false; // Completely disable this script in Breakroom
            return;
        }

        StartOpening();
    }

    private void Update()
    {
        switch (currentPhase)
        {
            case ShiftPhase.Opening:
                if (AllTasksComplete(openingTasks))
                {
                    CompleteOpening();
                }
                break;

            case ShiftPhase.Active:
                if (customerManager != null && customerManager.IsSpawningComplete())
                {
                    StopActivePhase();
                }
                break;

            case ShiftPhase.WaitingForCustomersToClear:
                if (customerManager != null && customerManager.AllCustomersCompleted())
                {
                    StartClosing();
                }
                break;

            case ShiftPhase.Closing:
                if (AllTasksComplete(closingTasks))
                {
                    CompleteClosing();
                }
                break;
        }
    }

    void StartOpening()
    {
        currentPhase = ShiftPhase.Opening;

        if (openingTaskUI != null) openingTaskUI.SetActive(true);
        if (closingTaskUI != null) closingTaskUI.SetActive(false);

        if (customerManager != null)
            customerManager.gameObject.SetActive(false);

        if (shelfManager != null)
            shelfManager.StopRemovingItems(); // Prevent stress from active shelf removal
    }

    void CompleteOpening()
    {
        if (openingTaskUI != null) openingTaskUI.SetActive(false);
        StartActiveShift();
    }

    void StartActiveShift()
    {
        currentPhase = ShiftPhase.Active;

        if (customerManager != null)
        {
            customerManager.gameObject.SetActive(true);
            customerManager.StartSpawning();
        }

        if (shelfManager != null)
            shelfManager.StartRemovingItems();

        Debug.Log("Active shift started: Customers spawning + shelf items disappearing.");
    }

    void StopActivePhase()
    {
        if (customerManager != null)
            customerManager.StopSpawning();

        if (shelfManager != null)
            shelfManager.StopRemovingItems();

        Debug.Log("ShiftSystem: Spawning and shelf removal ended. Waiting for customers to clear.");

        currentPhase = ShiftPhase.WaitingForCustomersToClear;
    }

    void StartClosing()
    {
        currentPhase = ShiftPhase.Closing;

        if (closingTaskUI != null) closingTaskUI.SetActive(true);

        Debug.Log("ShiftSystem: All customers completed. Closing tasks started.");
    }

    void CompleteClosing()
    {
        if (closingTaskUI != null)
            closingTaskUI.SetActive(false);

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
}










