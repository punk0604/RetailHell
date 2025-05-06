using UnityEngine;
using TMPro; // Make sure to include this if you're using TextMeshPro

public class ShiftConfirmationUI : MonoBehaviour
{
    private ShiftResetTrigger resetTrigger;

    [Header("UI Elements")]
    public GameObject confirmationPanel; // Assign the Confirmation Panel GameObject
    public string interactionPromptText = "Press E to Start Shift"; // Text for the interaction prompt

    private void Awake()
    {
        // Ensure the confirmation panel is inactive on start
        if (confirmationPanel != null)
        {
            confirmationPanel.SetActive(false);
        }
        else
        {
            Debug.LogError("ShiftConfirmationUI: confirmationPanel not assigned in the Inspector!");
        }
    }

    public void Show(ShiftResetTrigger trigger)
    {
        resetTrigger = trigger;
        if (confirmationPanel != null)
        {
            confirmationPanel.SetActive(true);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    public void Hide()
    {
        if (confirmationPanel != null)
        {
            confirmationPanel.SetActive(false);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked; // Re-lock the cursor when UI is hidden
        }
    }

    public void Confirm()
    {
        resetTrigger.ExecuteShiftReset();
        Hide();
    }

    public void Cancel()
    {
        Hide();
    }
}
