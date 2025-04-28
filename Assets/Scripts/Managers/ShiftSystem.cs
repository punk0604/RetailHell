using UnityEngine;
using UnityEngine.SceneManagement;

public class ShiftSystem : MonoBehaviour
{
    public enum ShiftPhase { Opening, Active, WaitingForCustomersToClear, Closing, Break }
    public ShiftPhase currentPhase;

    [Header("References")]
    public GameObject openingTasks;
    public GameObject closingTasks;
    public CustomerManager customerManager;
    public ShelfManager shelfManager; // ✅ Reference here

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
                    // ✅ Customers stop spawning after 2 minutes
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
        openingTasks.SetActive(true);
        closingTasks.SetActive(false);

        customerManager.gameObject.SetActive(false);
        if (shelfManager != null)
            shelfManager.StopRemovingItems(); // 🔥 No shelf removal during Opening
    }

    void CompleteOpening()
    {
        openingTasks.SetActive(false);
        StartActiveShift();
    }

    void StartActiveShift()
    {
        currentPhase = ShiftPhase.Active;
        customerManager.gameObject.SetActive(true);
        customerManager.StartSpawning();

        if (shelfManager != null)
            shelfManager.StartRemovingItems(); // ✅ Start damaging shelves

        Debug.Log("Active shift started: Customers spawning + shelf items disappearing.");
    }

    void StopActivePhase()
    {
        customerManager.StopSpawning();

        if (shelfManager != null)
            shelfManager.StopRemovingItems(); // ✅ STOP removing shelf items immediately

        Debug.Log("ShiftSystem: Spawning and shelf removal ended. Waiting for customers to clear.");

        currentPhase = ShiftPhase.WaitingForCustomersToClear;
    }

    void StartClosing()
    {
        currentPhase = ShiftPhase.Closing;
        closingTasks.SetActive(true);

        Debug.Log("ShiftSystem: All customers completed. Closing tasks started.");
    }

    void CompleteClosing()
    {
        closingTasks.SetActive(false);
        Debug.Log("Shift ended. Send to Breakroom.");
        //SceneManager.LoadScene("BreakRoom"); // Uncomment if ready
    }

    bool AllTasksComplete(GameObject taskParent)
    {
        foreach (Transform child in taskParent.transform)
        {
            if (child.gameObject.activeSelf) return false;
        }
        return true;
    }
}


