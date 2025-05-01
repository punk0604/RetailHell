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

        if (shelfManager == null)
        {
            Debug.LogError("ShelfGroup: No ShelfManager found in the scene!");
        }

        ShelfItem[] items = GetComponentsInChildren<ShelfItem>();

        if (items.Length == 0)
        {
            Debug.LogWarning($"ShelfGroup '{gameObject.name}' has no ShelfItems!");
        }

        foreach (ShelfItem item in items)
        {
            shelfItems.Add(item);
        }
    }


    private void Update()
    {
        if (!playerInZone) return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (shelfManager != null && shelfManager.CanRestock())
            {
                RestockAllItems();
                shelfManager.LockRestocking();
                Debug.Log($"✅ ShelfGroup '{gameObject.name}' restocked via player interaction.");
            }
            else
            {
                Debug.Log("🚫 Cannot restock shelf. Must return to stockpile first.");
                
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

        Debug.Log($"✅ All ShelfItems in '{gameObject.name}' have been restocked.");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInZone = true;
            Debug.Log($"ShelfGroup: Player entered restocking zone '{gameObject.name}'.");
            InteractionPromptUI.Instance?.Show("Press E to Restock");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInZone = false;
            Debug.Log($"ShelfGroup: Player exited restocking zone '{gameObject.name}'.");
            InteractionPromptUI.Instance?.Hide();        
        }
    }
}

