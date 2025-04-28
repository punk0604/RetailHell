using UnityEngine;
using System.Collections.Generic;

public class ShelfGroup : MonoBehaviour
{
    private List<ShelfItem> shelfItems = new List<ShelfItem>();
    private bool playerInZone = false;
    private ShelfManager shelfManager;

    private void Start()
    {
        shelfManager = FindObjectOfType<ShelfManager>();

        // Find all ShelfItems under this shelf
        ShelfItem[] items = GetComponentsInChildren<ShelfItem>();
        foreach (ShelfItem item in items)
        {
            shelfItems.Add(item);
        }
    }

    private void Update()
    {
        if (playerInZone && Input.GetKeyDown(KeyCode.E))
        {
            if (shelfManager != null && shelfManager.CanRestock())
            {
                RestockAllItems();
                shelfManager.LockRestocking();
                Debug.Log($"✅ Shelf '{gameObject.name}' restocked via trigger!");
            }
            else
            {
                Debug.LogWarning("🚫 Cannot restock. Visit stockpile first.");
            }
        }
    }

    public void RestockAllItems()
    {
        foreach (ShelfItem item in shelfItems)
        {
            if (!item.isStocked)
            {
                item.RestockItem();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInZone = true;
            Debug.Log($"Player entered shelf zone: {gameObject.name}");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInZone = false;
            Debug.Log($"Player exited shelf zone: {gameObject.name}");
        }
    }
}
