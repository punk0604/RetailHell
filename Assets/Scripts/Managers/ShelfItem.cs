using UnityEngine;

public class ShelfItem : MonoBehaviour
{
    public bool isStocked = true;

    public void RemoveItem()
    {
        isStocked = false;
        gameObject.SetActive(false); // visually hide the item
    }

    public void RestockItem()
    {
        isStocked = true;
        gameObject.SetActive(true);
    }


    public void ResetItem()
    {
        isStocked = true;
        gameObject.SetActive(true); // ensure the item is active at reset
    }
}


