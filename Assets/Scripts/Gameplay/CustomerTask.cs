using UnityEngine;

public class CustomerTask : MonoBehaviour
{
    public float waitTime = 20f;
    private float waitTimer;
    private bool taskComplete = false;
    private TaskStressManager stressManager;

    private void Start()
    {
        waitTimer = waitTime;
        stressManager = FindObjectOfType<TaskStressManager>();
        if (stressManager != null) stressManager.AddTask();
    }

    private void Update()
    {
        if (taskComplete) return;

        waitTimer -= Time.deltaTime;
        if (waitTimer <= 0f)
        {
            CompleteAndLeave(false);
        }
    }

    // ✅ PUBLIC method that can be called externally (e.g., from RegisterZone)
    public void CompleteTask()
    {
        if (!taskComplete)
        {
            CompleteAndLeave(true);
        }
    }

    private void CompleteAndLeave(bool successful)
    {
        taskComplete = true;

        if (successful)
            GameManager.Instance.AddPaycheck(5);

        if (stressManager != null)
            stressManager.RemoveTask();

        Destroy(gameObject);
    }

    // ✅ Optional helper for register zone
    public bool IsComplete()
    {
        return taskComplete;
    }
}





