using UnityEngine;
using System.Collections.Generic;

public class RegisterZone : MonoBehaviour
{
    private bool playerInZone = false;
    private List<CustomerTask> customersInZone = new List<CustomerTask>();

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInZone = true;
            TryClearCustomers();
        }

        CustomerTask customer = other.GetComponent<CustomerTask>();
        if (customer != null && !customersInZone.Contains(customer))
        {
            customersInZone.Add(customer);
            TryClearCustomers();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInZone = false;
        }

        CustomerTask customer = other.GetComponent<CustomerTask>();
        if (customer != null && customersInZone.Contains(customer))
        {
            customersInZone.Remove(customer);
        }
    }

    private void TryClearCustomers()
    {
        if (!playerInZone) return;

        for (int i = customersInZone.Count - 1; i >= 0; i--)
        {
            CustomerTask customer = customersInZone[i];
            if (customer != null && !customer.IsComplete())
            {
                customer.CompleteTask();
                customersInZone.RemoveAt(i);
                Debug.Log("✅ Customer cleared at register.");
            }
        }
    }
}
