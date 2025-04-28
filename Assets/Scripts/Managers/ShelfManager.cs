using System.Collections.Generic;
using UnityEngine;

public class ShelfManager : MonoBehaviour
{
    [Header("Shelf Settings")]
    public string shelfItemTag = "ShelfItem"; // Tag to look for
    public float itemDisappearInterval = 10f; // Time between item removals

    private List<ShelfItem> shelfItems = new List<ShelfItem>();
    private float timer = 0f;
    private bool canRemoveItems = false;
    private bool canRestock = false;


    private void Start()
    {
        FindShelfItems();
    }

    private void Update()
    {
        if (canRemoveItems)
        {
            timer -= Time.deltaTime;

            if (timer <= 0f)
            {
                RemoveRandomItem();
                timer = itemDisappearInterval;
            }
        }

        CheckRestockInput();
    }

    private void FindShelfItems()
    {
        GameObject[] items = GameObject.FindGameObjectsWithTag(shelfItemTag);

        foreach (GameObject obj in items)
        {
            ShelfItem shelfItem = obj.GetComponent<ShelfItem>();
            if (shelfItem != null)
            {
                shelfItems.Add(shelfItem);
            }
            else
            {
                Debug.LogWarning($"ShelfManager: Object '{obj.name}' tagged as {shelfItemTag} is missing a ShelfItem component.");
            }
        }

        Debug.Log($"ShelfManager: Found {shelfItems.Count} shelf items.");
    }

    public void StartRemovingItems()
    {
        if (shelfItems.Count == 0)
        {
            Debug.LogWarning("ShelfManager: No shelf items found to remove.");
            return;
        }

        canRemoveItems = true;
        timer = itemDisappearInterval;
        Debug.Log("ShelfManager: Started removing items.");
    }

    public void StopRemovingItems()
    {
        canRemoveItems = false;
        Debug.Log("ShelfManager: Stopped removing items.");
    }

    private void RemoveRandomItem()
    {
        List<ShelfItem> stockedItems = shelfItems.FindAll(item => item.isStocked);

        if (stockedItems.Count == 0)
        {
            Debug.Log("ShelfManager: No stocked items left to remove.");
            return;
        }

        int index = Random.Range(0, stockedItems.Count);
        ShelfItem itemToRemove = stockedItems[index];

        if (itemToRemove != null)
        {
            Debug.Log($"ShelfManager: Removing item '{itemToRemove.gameObject.name}' from shelf.");
            itemToRemove.RemoveItem();
        }
        else
        {
            Debug.LogWarning("ShelfManager: Tried to remove an item but it was null.");
        }
    }

    public void EnableRestocking()
    {
        canRestock = true;
    }

    public bool CanRestock()
    {
        return canRestock;
    }

    private void CheckRestockInput()
    {
        if (!canRestock)
            return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 3f)) // 3 units
            {
                ShelfGroup shelfGroup = hit.collider.GetComponentInParent<ShelfGroup>();

                if (shelfGroup != null)
                {
                    shelfGroup.RestockAllItems();
                    Debug.Log($" Shelf '{shelfGroup.gameObject.name}' restocked!");

                    // Immediately DISABLE restocking until player visits stockpile again
                    canRestock = false;
                    Debug.Log("Restocking locked. Player must return to stockpile.");
                }
                else
                {
                    Debug.Log(" No shelf group detected.");
                }
            }
        }
    }

    public void LockRestocking()
    {
        canRestock = false;
    }

}


