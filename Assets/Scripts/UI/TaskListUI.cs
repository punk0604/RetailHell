using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class TaskListUI : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject taskPanel;
    public Transform taskContainer;
    public GameObject taskTextPrefab;

    [Header("Active Phase Settings")]
    public CustomerManager customerManager;
    private TMP_Text customerCounterText;

    [Header("Task References")]
    public ShiftSystem shiftSystem;
    public LightSwitch lightSwitch;
    public CashRegister[] openingRegisters;
    public CashRegister[] closingRegisters;
    public ShelfManager shelfManager;

    private Dictionary<string, TMP_Text> currentTasks = new Dictionary<string, TMP_Text>();
    private ShiftSystem.ShiftPhase lastPhase;
    private TMP_Text restockText;

    private void Start()
    {
        lastPhase = shiftSystem.currentPhase;
        RefreshTaskList();
    }

    private void Update()
    {
        if (shiftSystem.currentPhase != lastPhase)
        {
            lastPhase = shiftSystem.currentPhase;
            RefreshTaskList();
        }

        if (shiftSystem.currentPhase == ShiftSystem.ShiftPhase.Active)
        {
            UpdateCustomerCounter();
        }

        UpdateTaskStatus();
    }

    void UpdateCustomerCounter()
    {
        if (customerCounterText != null && customerManager != null)
        {
            int remaining = customerManager.GetRemainingCustomers();
            customerCounterText.text = $"Customers Remaining: {remaining}";
        }
    }

    void RefreshTaskList()
    {
        foreach (Transform child in taskContainer)
            Destroy(child.gameObject);

        currentTasks.Clear();

        switch (shiftSystem.currentPhase)
        {
            case ShiftSystem.ShiftPhase.Opening:
                AddTask("Turn on Lights", lightSwitch.IsTaskComplete());
                AddTask("Open Registers", CountCompleted(openingRegisters), openingRegisters.Length);
                break;

            case ShiftSystem.ShiftPhase.Active:
                AddCustomerCounter();
                break;

            case ShiftSystem.ShiftPhase.Closing:
                AddTask("Turn off Lights", lightSwitch.IsTaskComplete());
                AddTask("Close Registers", CountCompleted(closingRegisters), closingRegisters.Length);
                AddRestockProgress();
                break;
        }
    }

    void AddTask(string taskName, bool isComplete)
    {
        GameObject taskObj = Instantiate(taskTextPrefab, taskContainer);
        TMP_Text text = taskObj.GetComponent<TMP_Text>();
        text.text = isComplete ? $"<s>{taskName}</s>" : taskName;
        currentTasks[taskName] = text;
    }

    void AddTask(string taskName, int current, int total)
    {
        GameObject taskObj = Instantiate(taskTextPrefab, taskContainer);
        TMP_Text text = taskObj.GetComponent<TMP_Text>();
        bool complete = (current >= total);
        text.text = complete ? $"<s>{taskName}: {current}/{total}</s>" : $"{taskName}: {current}/{total}";
        currentTasks[taskName] = text;
    }

    void AddRestockProgress()
    {
        GameObject taskObj = Instantiate(taskTextPrefab, taskContainer);
        restockText = taskObj.GetComponent<TMP_Text>();

        if (shelfManager != null)
        {
            int restocked = shelfManager.GetRestockedCount();
            int total = shelfManager.GetTotalShelfItemCount();
            restockText.text = $"Restock Shelves: {restocked} / {total}";
        }
        else
        {
            restockText.text = "Restock Shelves: 0 / ?";
        }
    }

    void UpdateTaskStatus()
    {
        switch (shiftSystem.currentPhase)
        {
            case ShiftSystem.ShiftPhase.Opening:
                if (currentTasks.ContainsKey("Turn on Lights"))
                    currentTasks["Turn on Lights"].text = lightSwitch.IsTaskComplete() ? "<s>Turn on Lights</s>" : "Turn on Lights";

                if (currentTasks.ContainsKey("Open Registers"))
                {
                    int opened = CountCompleted(openingRegisters);
                    bool complete = opened >= openingRegisters.Length;
                    currentTasks["Open Registers"].text = complete
                        ? $"<s>Open Registers: {opened}/{openingRegisters.Length}</s>"
                        : $"Open Registers: {opened}/{openingRegisters.Length}";
                }
                break;

            case ShiftSystem.ShiftPhase.Closing:
                if (currentTasks.ContainsKey("Turn off Lights"))
                    currentTasks["Turn off Lights"].text = lightSwitch.IsTaskComplete() ? "<s>Turn off Lights</s>" : "Turn off Lights";

                if (currentTasks.ContainsKey("Close Registers"))
                {
                    int closed = CountCompleted(closingRegisters);
                    bool complete = closed >= closingRegisters.Length;
                    currentTasks["Close Registers"].text = complete
                        ? $"<s>Close Registers: {closed}/{closingRegisters.Length}</s>"
                        : $"Close Registers: {closed}/{closingRegisters.Length}";
                }

                if (restockText != null && shelfManager != null)
                {
                    int restocked = shelfManager.GetRestockedCount();
                    int total = shelfManager.GetTotalShelfItemCount();
                    bool complete = restocked >= total;
                    restockText.text = complete
                        ? $"<s>Restock Shelves: {restocked} / {total}</s>"
                        : $"Restock Shelves: {restocked} / {total}";
                }
                break;

            case ShiftSystem.ShiftPhase.Active:
                UpdateCustomerCounter();
                break;
        }
    }

    int CountCompleted(CashRegister[] registers)
    {
        int count = 0;
        foreach (var reg in registers)
        {
            if (reg != null && reg.IsTaskComplete()) count++;
        }
        return count;
    }

    private void AddCustomerCounter()
    {
        GameObject taskObj = Instantiate(taskTextPrefab, taskContainer);
        customerCounterText = taskObj.GetComponent<TMP_Text>();
        customerCounterText.text = "Customers Remaining: 0";
    }
}


