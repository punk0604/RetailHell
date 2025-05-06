using System.Collections.Generic;
using UnityEngine;

public class ShelfManager : MonoBehaviour, ShiftTask
{
    [Header("Shelf Settings")]
    public string shelfItemTag = "ShelfItem"; // Tag to find shelf items
    public float itemDisappearInterval = 10f; // Interval between random removals

    private List<ShelfItem> shelfItems = new List<ShelfItem>();
    private float timer = 0f;
    private bool canRemoveItems = false;
    private bool canRestock = false;

    public ShiftSystem shiftSystem; // Link your ShiftSystem in Inspector

    public ShiftSystem.ShiftPhase TaskPhase => ShiftSystem.ShiftPhase.Closing;
    public bool CanRestock()
    {
        return canRestock;
    }


    private void Start()
    {
        FindShelfItems();
    }

    private void Update()
    {
        if (shiftSystem == null) return;

        switch (shiftSystem.currentPhase)
        {
            case ShiftSystem.ShiftPhase.Active:
                if (canRemoveItems)
                {
                    timer -= Time.deltaTime;

                    if (timer <= 0f)
                    {
                        RemoveRandomItem();
                        timer = itemDisappearInterval;
                    }
                }
                break;

            case ShiftSystem.ShiftPhase.Closing:
                CheckRestockInput();
                break;
        }
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
        Debug.Log("ShelfManager: Started removing items during Active phase.");
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
        Debug.Log("ShelfManager: Player can now restock shelves.");
    }

    public void LockRestocking()
    {
        canRestock = false;
    }

    private void CheckRestockInput()
    {
        if (!canRestock) return;

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
                    Debug.Log($"✅ Shelf '{shelfGroup.gameObject.name}' restocked!");

                    // After restocking, require the player to return to stockpile
                    canRestock = false;
                    Debug.Log("ShelfManager: Restocking locked. Return to stockpile to restock again.");
                }
                else
                {
                    Debug.Log("ShelfManager: No valid shelf detected.");
                }
            }
        }
    }

    private bool AreShelvesRestocked()
    {
        foreach (ShelfItem item in shelfItems)
        {
            if (!item.isStocked)
                return false;
        }
        return true;
    }

    public bool IsTaskComplete()
    {
        return AreShelvesRestocked();
    }
    
    public int GetTotalShelfItemCount()
    {
        return shelfItems.Count;
    }

    public int GetRestockedCount()
    {
        int count = 0;
        foreach (ShelfItem item in shelfItems)
        {
            if (item.isStocked) count++;
        }
        return count;
    }
    public void ResetTask()
    {
        foreach (ShelfItem item in shelfItems)
        {
            item.ResetItem();
        }
    }



}



