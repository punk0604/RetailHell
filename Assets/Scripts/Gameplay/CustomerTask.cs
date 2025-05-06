using UnityEngine;

public class CustomerTask : MonoBehaviour
{
    public float waitTime = 20f;
    private float waitTimer;
    private bool taskComplete = false;
    private TaskStressManager stressManager;
    public bool playerInRange = false;

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

        /*if (stressManager != null)
        { stressManager.RemoveTask(); }*/
        if (successful)
        { GameManager.Instance.AddPaycheck(5); }

        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            Debug.Log("Stockpile: Player ENTERED customer range.");
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            Debug.Log("Stockpile: Player EXITED customer range.");
        }
    }
}










