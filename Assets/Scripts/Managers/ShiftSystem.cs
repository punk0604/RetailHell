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
    public string openingTasksName = "Tasks";
    public string closingTasksName = "Tasks";

    [Header("System Names")]
    public string customerManagerName = "CustomerManager";
    public string shelfManagerName = "ShelfManager";

    // Cached references
    public GameObject openingTaskUI { get; private set; }
    public GameObject closingTaskUI { get; private set; }
    public GameObject openingTasks { get; private set; }
    public GameObject closingTasks { get; private set; }
    public CustomerManager customerManager { get; private set; }
    public ShelfManager shelfManager { get; private set; }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"ShiftSystem: Scene loaded: {scene.name}");

        if (scene.name != "breakRoom")
        {
            // Find dependencies in the newly loaded scene
            Awake();

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

    private void Awake()
    {
        if (openingTasks == null)
            openingTasks = GameObject.Find("Tasks"); 
        if (closingTasks == null)
            closingTasks = GameObject.Find("Tasks");

        if (openingTaskUI == null)
            openingTaskUI = GameObject.Find("OpeningTaskUI");

        if (closingTaskUI == null)
            closingTaskUI = GameObject.Find("ClosingTaskUI");

        if (customerManager == null)
            customerManager = FindObjectOfType<CustomerManager>();

        if (shelfManager == null)
            shelfManager = FindObjectOfType<ShelfManager>();
    }


    private void Start()
    {
        // Find dependencies on initial scene load as well
        if (SceneManager.GetActiveScene().name != "breakRoom")
        {
            Awake();
            StartOpening();
        }
    }

    private void Update()
    {
        switch (currentPhase)
        {
            case ShiftPhase.Opening:
                if (AllTasksComplete(openingTasks)) CompleteOpening();
                break;
            case ShiftPhase.Active:
                if (customerManager != null && customerManager.IsSpawningComplete()) StopActivePhase();
                break;
            case ShiftPhase.WaitingForCustomersToClear:
                if (customerManager != null && customerManager.AllCustomersCompleted()) StartClosing();
                break;
            case ShiftPhase.Closing:
                if (AllTasksComplete(closingTasks)) CompleteClosing();
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

        if (openingTasks == null)
            Debug.LogWarning("ShiftSystem: OpeningTasks not set at StartOpening.");
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

    void ResetShift()
    {
        void ResetShift()
{
    currentPhase = ShiftPhase.Opening;
    StartOpening();
}

    }


}











