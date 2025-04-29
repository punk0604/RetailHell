using UnityEngine;
using UnityEngine.SceneManagement;

public class ShiftSystem : MonoBehaviour
{
    public enum ShiftPhase { Opening, Active, WaitingForCustomersToClear, Closing, Break }
    public ShiftPhase currentPhase;

    [Header("References")]
    public GameObject openingTaskUI; // 🟡 Just the UI (not physical tasks)
    public GameObject closingTaskUI; // 🟡 Just the UI (not physical tasks)

    public GameObject openingTasks; // 🟢 Parent of physical task scripts (DON'T deactivate)
    public GameObject closingTasks; // 🟢 Parent of physical task scripts (DON'T deactivate)

    public CustomerManager customerManager;
    public ShelfManager shelfManager;

    private void Start()
    {
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

        customerManager.gameObject.SetActive(false);

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

        customerManager.gameObject.SetActive(true);
        customerManager.StartSpawning();

        if (shelfManager != null)
            shelfManager.StartRemovingItems();

        Debug.Log("Active shift started: Customers spawning + shelf items disappearing.");
    }

    void StopActivePhase()
    {
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
        if (closingTaskUI != null) closingTaskUI.SetActive(false);
        Debug.Log("✅ Shift ended. Send to Breakroom.");
        //SceneManager.LoadScene("BreakRoom");
    }

    bool AllTasksComplete(GameObject taskParent)
    {
        ShiftTask[] tasks = taskParent.GetComponentsInChildren<ShiftTask>(true);

        foreach (ShiftTask task in tasks)
        {
            if (task.TaskPhase == currentPhase && !task.IsTaskComplete())
                return false;
        }

        return true;
    }
}



