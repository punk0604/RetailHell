using UnityEngine;
using UnityEngine.SceneManagement;

public class ShiftSystem : MonoBehaviour
{
    public enum ShiftPhase { Opening, Active, Closing, Break }
    public ShiftPhase currentPhase;

    [Header("References")]
    public GameObject openingTasks;
    public GameObject closingTasks;
    public CustomerManager customerManager;

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
                // ✅ Let CustomerManager handle spawn timing
                // Just check when all customers are gone
                if (customerManager != null && customerManager.IsSpawningComplete() && customerManager.AllCustomersCompleted())
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
        customerManager.StartSpawning(); // ✅ Let CustomerManager handle its own timing
        Debug.Log("Active shift started: Customers will spawn internally for 2 minutes.");
    }

    void StartClosing()
    {
        currentPhase = ShiftPhase.Closing;
        closingTasks.SetActive(true);
        Debug.Log("All customers completed. Closing tasks started.");
    }

    void CompleteClosing()
    {
        closingTasks.SetActive(false);
        //SceneManager.LoadScene("BreakRoom");
        Debug.Log("Shift ended. Send to Breakroom.");
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




