using UnityEngine;
using UnityEngine.SceneManagement;

public class ShiftSystem : MonoBehaviour
{
    public enum ShiftPhase { Opening, Active, WaitingForCustomersToClear, Closing, Break }
    public ShiftPhase currentPhase;

    [Header("UI Names")]
    public string openingTaskUIName = "OpeningTaskUI";
    public string closingTaskUIName = "ClosingTaskUI";

    [Header("Task Parent Names")]
    public string taskParentName = "Tasks"; // Unified task object

    [Header("System Names")]
    public string customerManagerName = "CustomerManager";
    public string shelfManagerName = "ShelfManager";

    [HideInInspector] public GameObject openingTaskUI;
    [HideInInspector] public GameObject closingTaskUI;
    [HideInInspector] public GameObject tasks;
    [HideInInspector] public CustomerManager customerManager;
    [HideInInspector] public ShelfManager shelfManager;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Awake()
    {
        AssignReferences();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        AssignReferences();

        if (scene.name != "breakRoom")
        {
            if (currentPhase == ShiftPhase.Break)
            {
                Debug.Log("Returning from Breakroom. Resetting shift.");
                ResetShift();
            }
        }
        else
        {
            Debug.Log("ShiftSystem disabled in Breakroom scene.");
            enabled = false;
        }
    }

    private void Start()
    {
        if (SceneManager.GetActiveScene().name != "breakRoom")
        {
            StartOpening();
        }
    }

    void AssignReferences()
    {
        tasks = GameObject.Find(taskParentName);
        openingTaskUI = GameObject.Find(openingTaskUIName);
        closingTaskUI = GameObject.Find(closingTaskUIName);
        customerManager = FindObjectOfType<CustomerManager>();
        shelfManager = FindObjectOfType<ShelfManager>();
    }

    private void Update()
    {
        switch (currentPhase)
        {
            case ShiftPhase.Opening:
                if (AllTasksComplete(tasks)) CompleteOpening();
                break;
            case ShiftPhase.Active:
                if (customerManager != null && customerManager.IsSpawningComplete()) StopActivePhase();
                break;
            case ShiftPhase.WaitingForCustomersToClear:
                if (customerManager != null && customerManager.AllCustomersCompleted()) StartClosing();
                break;
            case ShiftPhase.Closing:
                if (AllTasksComplete(tasks)) CompleteClosing();
                break;
        }
    }

    void StartOpening()
    {
        currentPhase = ShiftPhase.Opening;
        if (openingTaskUI != null) openingTaskUI.SetActive(true);
        if (closingTaskUI != null) closingTaskUI.SetActive(false);
        if (customerManager != null) customerManager.gameObject.SetActive(false);
        if (shelfManager != null) shelfManager.StopRemovingItems();
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
        if (shelfManager != null) shelfManager.StartRemovingItems();

        Debug.Log("Active shift started: Customers spawning + shelf items disappearing.");
    }

    void StopActivePhase()
    {
        if (customerManager != null) customerManager.StopSpawning();
        if (shelfManager != null) shelfManager.StopRemovingItems();
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
        currentPhase = ShiftPhase.Break;
        Debug.Log("✅ Shift ended. Breakroom is now accessible.");
    }

    bool AllTasksComplete(GameObject taskParent)
    {
        if (taskParent == null)
        {
            Debug.LogWarning("ShiftSystem: taskParent is null.");
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

    public void ResetShift()
    {
        Debug.Log("🔁 ShiftSystem: Resetting shift after Breakroom.");
        AssignReferences(); // Ensure all tasks are found again
        currentPhase = ShiftPhase.Opening;

        if (openingTaskUI != null) openingTaskUI.SetActive(false);
        if (closingTaskUI != null) closingTaskUI.SetActive(false);

        if (tasks != null)
        {
            foreach (ShiftTask task in tasks.GetComponentsInChildren<ShiftTask>(true))
            {
                task.ResetTask();
            }
        }

        StartOpening();
    }
}












