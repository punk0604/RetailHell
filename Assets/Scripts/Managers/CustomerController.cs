/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CustomerController : MonoBehaviour
{
    [Header("Customer Settings")]
    public GameObject[] customerPrefabs;  // Assign 3D model prefabs
    public Transform entryPoint;          // Spawn point
    public Transform[] shelfTargets;      // Shelf destinations
    public Transform checkoutTarget;      // Register1
    public float browseTime = 3f;

    [Header("Spawn Control")]
    public float spawnInterval = 5f;
    public float spawnDuration = 120f;

    private float spawnTimer;
    private float intervalTimer;
    private bool isSpawning = false;
    private Queue<GameObject> activeCustomers = new Queue<GameObject>();

    private TaskStressManager stressManager;
    public GameObject clock;

    void Start()
    {
        stressManager = FindObjectOfType<TaskStressManager>();
    }

    void Update()
    {
        if (!isSpawning) return;

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
        }
    }

    public void StartSpawning()
    {
        isSpawning = true;
        spawnTimer = spawnDuration;
        intervalTimer = spawnInterval;
        clock.SetActive(true);
        Debug.Log("CustomerController: Spawning started.");
    }

    public void StopSpawning()
    {
        isSpawning = false;
        clock.SetActive(false);
        Debug.Log("CustomerController: Spawning ended.");
    }

    void SpawnCustomer()
    {
        if (customerPrefabs.Length == 0 || shelfTargets.Length == 0 || entryPoint == null || checkoutTarget == null)
        {
            Debug.LogWarning("CustomerController: Missing setup references!");
            return;
        }

        GameObject randomPrefab = customerPrefabs[Random.Range(0, customerPrefabs.Length)];
        GameObject customer = Instantiate(randomPrefab, entryPoint.position, Quaternion.identity);
        NavMeshAgent agent = customer.GetComponent<NavMeshAgent>();

        if (agent != null)
        {
            Transform shelf = shelfTargets[Random.Range(0, shelfTargets.Length)];
            StartCoroutine(CustomerRoutine(customer, agent, shelf, checkoutTarget));
        }

        activeCustomers.Enqueue(customer);
        stressManager?.AddTask();
    }

    IEnumerator CustomerRoutine(GameObject customer, NavMeshAgent agent, Transform shelf, Transform register)
    {
        // Go to shelf
        agent.SetDestination(shelf.position);
        yield return new WaitUntil(() => !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance);

        yield return new WaitForSeconds(browseTime);

        // Go to register
        agent.SetDestination(register.position);
        yield return new WaitUntil(() => !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance);

        // Done - remove task and customer
        stressManager?.RemoveTask();
        activeCustomers.Dequeue();
        Destroy(customer);
    }

    public bool AllCustomersCompleted() => activeCustomers.Count == 0;
    public bool IsSpawningComplete() => !isSpawning;
    public int GetRemainingCustomers() => activeCustomers.Count;
}
*/