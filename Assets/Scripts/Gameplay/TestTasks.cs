using UnityEngine;

public class GenericTask : MonoBehaviour
{
    private bool taskComplete = false;
    private TaskStressManager stressManager;

    private void Start()
    {
        stressManager = FindObjectOfType<TaskStressManager>();
        if (stressManager != null)
        {
            stressManager.AddTask();
        }
    }

    private void Update()
    {
        if (!taskComplete && Input.GetKeyDown(KeyCode.J))
        {
            CompleteTask();
        }
    }

    private void CompleteTask()
    {
        taskComplete = true;

        if (stressManager != null)
        {
            stressManager.RemoveTask();
        }

        Debug.Log($"{gameObject.name} task completed.");
        gameObject.SetActive(false); // 🔁 Deactivates instead of destroying
    }
}

