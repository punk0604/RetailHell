using UnityEngine;
using UnityEngine.Events;

public class TaskStressManager : MonoBehaviour
{
    [Header("Link to your Progress Bar")]
    public ProgressBar stressBar;

    [Header("Stress Settings")]
    public float stressPerTask = 10f;
    public float maxStress = 100f;

    [Header("Events")]
    public UnityEvent onStressMaxed; 

    private int currentTasks = 0;
    private float targetStress = 0f;

    private void Update()
    {
        if (stressBar != null && !Mathf.Approximately(stressBar.BarValue, targetStress))
        {
            stressBar.BarValue = Mathf.Lerp(stressBar.BarValue, targetStress, Time.deltaTime * 5f);
        }
    }

    public void AddTask()
    {
        currentTasks++;
        Debug.Log("Task added. Total: " + currentTasks);
        UpdateStressBar();
    }

    public void RemoveTask()
    {
        if (currentTasks > 0)
        {
            currentTasks--;
            Debug.Log("Task removed. Total: " + currentTasks);
            UpdateStressBar();
        }
    }

    private void UpdateStressBar()
    {
        float stress = currentTasks * stressPerTask;
        targetStress = Mathf.Min(stress, maxStress);

        if (targetStress >= maxStress)
        {
            Debug.Log("Stress maxed! Player quits.");
            onStressMaxed?.Invoke();
        }
    }

    public int GetCurrentTasks()
    {
        return currentTasks;
    }

    public void IncreaseStressCap(int amount)
    {
        maxStress += amount;
        Debug.Log($"New max stress: {maxStress}");
    }

    public void DisableStressForShift()
    {
        enabled = false;
        Debug.Log("Stress disabled for current shift.");
    }

    public void ResetStress()
    {
        currentTasks = 0;
        targetStress = 0f;

        if (stressBar != null)
        {
            stressBar.BarValue = 0f;
        }

        Debug.Log("Stress reset to 0.");
    }



}



