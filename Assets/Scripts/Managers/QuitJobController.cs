using UnityEngine;

public class QuitGameInteraction : MonoBehaviour
{
    public float interactRange = 3f;
    public float holdDuration = 2f; // seconds to hold E
    private float holdTimer = 0f;
    
    void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactRange))
        {
            if (hit.collider.CompareTag("QuitObject"))
            {
                if (Input.GetKey(KeyCode.E))
                {
                    holdTimer += Time.deltaTime;
                    if (holdTimer >= holdDuration)
                    {
                        Debug.Log("Quitting game...");
                    }
                }
                else
                {
                    // Reset timer if E is released early
                    holdTimer = 0f;
                }
            }
            else
            {
                holdTimer = 0f;
            }
        }
        else
        {
            holdTimer = 0f;
        }
    }
}

