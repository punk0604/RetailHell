using System.Collections.Generic;
using UnityEngine;

public class CustomerManager : MonoBehaviour
{
    public GameObject customerPrefab;
    public Transform[] spawnPoints;
    public float spawnInterval = 5f;

    private float timer = 0f;
    private Queue<GameObject> activeCustomers = new Queue<GameObject>();

    private void Update()
    {
        if (!gameObject.activeInHierarchy) return; // 🚫 Prevents spawning when inactive

        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            SpawnCustomer();
            timer = spawnInterval;
        }

        // Press J to complete the oldest customer
        if (Input.GetKeyDown(KeyCode.J) && activeCustomers.Count > 0)
        {
            GameObject oldest = activeCustomers.Dequeue();
            CustomerTask task = oldest.GetComponent<CustomerTask>();
            if (task != null)
            {
                task.CompleteTask();
            }
        }
    }

    private void SpawnCustomer()
    {
        int index = Random.Range(0, spawnPoints.Length);
        GameObject customer = Instantiate(customerPrefab, spawnPoints[index].position, Quaternion.identity);
        activeCustomers.Enqueue(customer);
        Debug.Log("Customer spawned. Total active: " + activeCustomers.Count);
    }
}


