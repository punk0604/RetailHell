using System.Collections.Generic;
using UnityEngine;

public class CustomerManager : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject customerPrefab;
    public Transform[] spawnPoints;
    public float spawnInterval = 5f;
    public float spawnDuration = 60f; // Total allowed spawning time (2 minutes)

    private float spawnTimer = 0f;
    private float intervalTimer = 0f;
    private bool isSpawningAllowed = false;

    private Queue<GameObject> activeCustomers = new Queue<GameObject>();

    public void StartSpawning()
    {
        isSpawningAllowed = true;
        spawnTimer = spawnDuration;
        intervalTimer = spawnInterval;
        Debug.Log("CustomerManager: Spawning started for " + spawnDuration + " seconds.");
    }

    public void StopSpawning()
    {
        isSpawningAllowed = false;
        Debug.Log("CustomerManager: Spawning ended.");
    }

    public bool AllCustomersCompleted()
    {
        return activeCustomers.Count == 0;
    }

    public bool IsSpawningComplete()
    {
        return !isSpawningAllowed;
    }

    private void Update()
    {
        if (!isSpawningAllowed)
            return;

        spawnTimer -= Time.deltaTime;
        intervalTimer -= Time.deltaTime;

        if (spawnTimer <= 0f)
        {
            StopSpawning();
            return;
        }

        if (intervalTimer <= 0f)
        {
            SpawnCustomer();
            intervalTimer = spawnInterval;

            Debug.Log($"CustomerManager: Customer spawned. Time left: {spawnTimer:F2} seconds.");
        }

        // Press J to complete the oldest customer
        if (Input.GetKeyDown(KeyCode.J) && activeCustomers.Count > 0)
        {
            GameObject oldest = activeCustomers.Dequeue();
            if (oldest != null)
            {
                CustomerTask task = oldest.GetComponent<CustomerTask>();
                if (task != null)
                {
                    Debug.Log("CustomerManager: Completing oldest customer.");
                    task.CompleteTask();
                }
            }
        }
    }

    private void SpawnCustomer()
    {
        if (customerPrefab == null)
        {
            Debug.LogError("CustomerManager: Customer prefab not assigned!");
            return;
        }

        if (spawnPoints.Length == 0)
        {
            Debug.LogError("CustomerManager: No spawn points assigned!");
            return;
        }

        int index = Random.Range(0, spawnPoints.Length);
        GameObject customer = Instantiate(customerPrefab, spawnPoints[index].position, Quaternion.identity);
        activeCustomers.Enqueue(customer);

        Debug.Log("CustomerManager: New customer added. Active queue count: " + activeCustomers.Count);
    }
}






