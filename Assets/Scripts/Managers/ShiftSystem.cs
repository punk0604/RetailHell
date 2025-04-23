using UnityEngine;
using UnityEngine.SceneManagement;

public class ShiftSystem : MonoBehaviour
{
    public enum ShiftPhase { Opening, Active, Closing, Break }
    public ShiftPhase currentPhase;

    [Header("Phase Timing")]
    public float activeShiftDuration = 300f; // 5 minutes
    private float shiftTimer;

    [Header("References")]
    public GameObject openingTasks;
    public GameObject closingTasks;
    public GameObject customerManager;

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
                shiftTimer -= Time.deltaTime;
                if (shiftTimer <= 0f)
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

        // Optional debug keys
        if (Input.GetKeyDown(KeyCode.O)) CompleteOpening();
        if (Input.GetKeyDown(KeyCode.C)) CompleteClosing();
    }

    void StartOpening()
    {
        currentPhase = ShiftPhase.Opening;
        openingTasks.SetActive(true);
        closingTasks.SetActive(false);
        customerManager.SetActive(false); // 🔒 Prevent spawning

        Debug.Log("Shift started: Opening phase.");
    }

    void CompleteOpening()
    {
        openingTasks.SetActive(false);
        StartActiveShift();
    }

    void StartActiveShift()
    {
        currentPhase = ShiftPhase.Active;
        shiftTimer = activeShiftDuration;

        customerManager.SetActive(true); // ✅ Customers now spawn
        Debug.Log("Opening complete. Active shift started.");
    }

    void StartClosing()
    {
        currentPhase = ShiftPhase.Closing;
        customerManager.SetActive(false);
        closingTasks.SetActive(true);

        Debug.Log("Active shift complete. Closing tasks started.");
    }

    void CompleteClosing()
    {
        closingTasks.SetActive(false);
        GoToBreakRoom();
    }

    void GoToBreakRoom()
    {
        currentPhase = ShiftPhase.Break;
        Debug.Log("Shift complete. Loading breakroom...");
        //SceneManager.LoadScene("BreakRoom");
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


