using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CustomerManager : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject customerPrefab;
    public Transform[] spawnPoints;
    public float spawnInterval = 5f;
    public float spawnDuration = 120f; // Total allowed spawning time (2 minutes)

    private float spawnTimer = 0f;
    private float intervalTimer = 0f;
    private bool isSpawningAllowed = false;
    //private bool playerInRange = false;

    private static int numCustomers = 10;
    //private Queue<GameObject> activeCustomers = new Queue<GameObject>();
    private GameObject[] activeCustomers = new GameObject[numCustomers];
    private static TaskStressManager stressManager;

    public GameObject clock;
    public GameObject voice;
    public GameObject serve;
    private GameObject currentCustomer;

    private void Awake()
    {
        // Ensure there's only one CustomerManager in the scene
        CustomerManager[] managers = FindObjectsOfType<CustomerManager>();
        if (managers.Length > 1)
        {
            Debug.LogError("CustomerManager: Multiple instances of CustomerManager found in the scene! This could lead to unexpected behavior. Destroying this instance.");
            Destroy(gameObject);
            return;
        }

        // Find TaskStressManager on Awake to ensure it's found before Start
        stressManager = FindObjectOfType<TaskStressManager>();
        if (stressManager == null)
        {
            Debug.LogError("CustomerManager: TaskStressManager not found in the scene. Customer completion will likely fail.");
        }

        if (customerPrefab == null)
        {
            Debug.LogError("CustomerManager: Customer prefab is not assigned in the Inspector!");
        }

        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogError("CustomerManager: No spawn points are assigned in the Inspector!");
        }
        else
        {
            Debug.Log("CustomerManager: Found " + spawnPoints.Length + " spawn points.");
        }

        if (clock == null)
        {
            Debug.LogWarning("CustomerManager: Clock GameObject is not assigned. Clock-related functionality will not work.");
        }
    }

    private void Start()
    {
        Debug.Log("CustomerManager: Start method called.");
    }

    public void StartSpawning()
    {
        if (clock != null)
        {
            clock.SetActive(true);
            AudioSource clockAudio = clock.GetComponent<AudioSource>();
            if (clockAudio != null)
            {
                Debug.Log("CustomerManager: Clock activated.");
            }
            else
            {
                Debug.LogWarning("CustomerManager: Clock GameObject does not have an AudioSource component.");
            }
        }
        else
        {
            Debug.LogWarning("CustomerManager: StartSpawning called, but clock GameObject is not assigned.");
        }

        isSpawningAllowed = true;
        spawnTimer = spawnDuration;
        intervalTimer = spawnInterval;
        Debug.Log("CustomerManager: Spawning started for " + spawnDuration + " seconds.");
    }

    public void StopSpawning()
    {
        isSpawningAllowed = false;
        if (clock != null)
        {
            AudioSource clockAudio = clock.GetComponent<AudioSource>();
            if (clockAudio != null)
            {
                clockAudio.Play();
                Debug.Log("CustomerManager: Clock sound played on stopping.");
            }
            else
            {
                Debug.LogWarning("CustomerManager: Clock GameObject does not have an AudioSource component, cannot play stop sound.");
            }
        }
        else
        {
            Debug.LogWarning("CustomerManager: StopSpawning called, but clock GameObject is not assigned, cannot play stop sound.");
        }
        Debug.Log("CustomerManager: Spawning ended.");
    }

    public bool AllCustomersCompleted()
    {
        bool allCompleted = false;
        int remaining = GetRemainingCustomers();
        if (remaining == 0)
        {
            return allCompleted = true;
        }
        Debug.Log("CustomerManager: AllCustomersCompleted called. Active customer count: " + remaining);
        return allCompleted;

    }

    public bool IsSpawningComplete()
    {
        Debug.Log("CustomerManager: IsSpawningComplete called. Spawning allowed: " + isSpawningAllowed + ". Returning: " + !isSpawningAllowed);
        return !isSpawningAllowed;
    }

    private void Update()
    {
        if (!isSpawningAllowed)
        {
            // Debug log when Update is called but spawning is not allowed
            // This can help track if Update is running when it shouldn't be
            // or if isSpawningAllowed is not being set correctly.
            // Debug.Log("CustomerManager: Update called, but spawning is not allowed.");
            return;
        }

        spawnTimer -= Time.deltaTime;
        intervalTimer -= Time.deltaTime;
        // Debug.Log($"CustomerManager: Update - Spawn Timer: {spawnTimer:F2}, Interval Timer: {intervalTimer:F2}");

        if (spawnTimer <= 0f)
        {
            Debug.Log("CustomerManager: Spawn timer reached zero. Calling StopSpawning.");
            StopSpawning();
            return;
        }

        if (intervalTimer <= 0f)
        {
            Debug.Log("CustomerManager: Interval timer reached zero. Attempting to spawn a customer.");
            SpawnCustomer();
            intervalTimer = spawnInterval;
        }

        for (int i = 0; i < 10; i++) {
            int remaining = GetRemainingCustomers();
            if (activeCustomers[i] != null) {
                GameObject currentCustomer = activeCustomers[i];
                if (currentCustomer.GetComponent<CustomerTask>().playerInRange == true)
                {
                    InteractionPromptUI.Instance?.Show("Press E to Serve Customer");

                    // Press J to complete the oldest customer
                    if (Input.GetKeyDown(KeyCode.E) && remaining > 0) // && (customerPrefab.GetComponent<CustomerTask>().playerInRange == true)
                    {
                        Debug.Log("CustomerManager: 'E' key pressed. Attempting to complete the customer. Active customer count: " + activeCustomers.Length);
                        
                        serve.GetComponent<AudioSource>().Play(); //play ka-ching sound
                        if (stressManager != null)
                        {
                            stressManager.RemoveTask();
                            Debug.Log("CustomerManager: Removed a task from StressManager.");
                        }
                        else
                        {
                            Debug.LogWarning("CustomerManager: StressManager is null, cannot remove task.");
                        }

                        if (currentCustomer != null)
                        {
                            CustomerTask task = currentCustomer.GetComponent<CustomerTask>();
                            if (task != null)
                            {
                                Debug.Log("CustomerManager: Found CustomerTask component on the oldest customer. Calling CompleteTask.");
                                task.CompleteTask();
                                activeCustomers[i] = null;
                            }
                            else
                            {
                                Debug.LogWarning("CustomerManager: Oldest customer does not have a CustomerTask component.");
                            }
                        }
                        else
                        {
                            Debug.LogError("CustomerManager: Dequeued customer is null!");
                        }
                    }
                    else if (Input.GetKeyDown(KeyCode.J) && remaining == 0)
                    {
                        Debug.Log("CustomerManager: 'J' key pressed, but no active customers to complete.");
                    }
                }
            }
        }
    }

    private void SpawnCustomer()
    {
        if (customerPrefab == null)
        {
            Debug.LogError("CustomerManager: SpawnCustomer called, but customer prefab is still not assigned!");
            return;
        }

        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogError("CustomerManager: SpawnCustomer called, but no spawn points are assigned!");
            return;
        }

        int index = UnityEngine.Random.Range(0, 10);
        Transform spawnTransform = spawnPoints[index];
        
        if (spawnTransform == null)
        {
            Debug.LogError("CustomerManager: Selected spawn point at index " + index + " is null!");
            return;
        }

        GameObject customer = Instantiate(customerPrefab, spawnTransform.position, Quaternion.identity);
        voice.GetComponent<AudioSource>().Play(); //play gibberish sound
        if (customer != null)
        {
            activeCustomers[index] = customer;
            Debug.Log("CustomerManager: New customer instantiated at " + spawnTransform.position + ". Active queue count: " + activeCustomers.Length);

            // Optional: Add a CustomerTask component if it doesn't exist
            CustomerTask taskComponent = customer.GetComponent<CustomerTask>();
            if (taskComponent == null)
            {
                Debug.LogWarning("CustomerManager: Instantiated customer does not have a CustomerTask component. Adding one.");
                customer.AddComponent<CustomerTask>();
            }
        }
        else
        {
            Debug.LogError("CustomerManager: Failed to instantiate customer prefab!");
        }
    }

    public int GetRemainingCustomers()
    {
        int remaining = 0;
        for(int i = 0; i<10; i++)
        {
            if(activeCustomers[i] != null)
            {
                remaining++;
            }
        }
        Debug.Log("CustomerManager: GetRemainingCustomers called. Returning: " + remaining);
        return remaining;
    }
}










