using UnityEngine;

public class TaskStressManager : MonoBehaviour
{
    [Header("Link to your Progress Bar")]
    public ProgressBar stressBar;

    [Header("Stress Settings")]
    public float stressPerTask = 10f;
    public float maxStress = 100f;

    private int currentTasks = 0;

    public void AddTask()
    {
        currentTasks++;
        UpdateStressBar();
    }

    public void RemoveTask()
    {
        if (currentTasks > 0)
        {
            currentTasks--;
            UpdateStressBar();
        }
    }

    private void UpdateStressBar()
    {
        float stress = currentTasks * stressPerTask;
        stressBar.BarValue = Mathf.Min(stress, maxStress);
    }
}

