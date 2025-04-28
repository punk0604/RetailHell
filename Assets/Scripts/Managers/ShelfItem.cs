using UnityEngine;

public class ShelfItem : MonoBehaviour
{
    public bool isStocked = true; // ✅ Track if this item is available or missing

    public void RemoveItem()
    {
        isStocked = false;
        gameObject.SetActive(false); // ✅ Hide the item visually
    }

    public void RestockItem()
    {
        isStocked = true;
        gameObject.SetActive(true); // ✅ Bring the item back visually
    }
}

