using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class VendingMachineUI : MonoBehaviour
{
    [Header("Prices")]
    public int energyDrinkPrice = 10;
    public int snackBarPrice = 5;
    public int zenSodaPrice = 15;

    [Header("UI Text")]
    public TMP_Text energyDrinkCountText;
    public TMP_Text snackBarCountText;
    public TMP_Text zenSodaCountText;
    public TMP_Text totalText;
    public TMP_Text paycheckText;

    [Header("References")]
    public GameObject vendingUI; // Root panel of vending UI
    public PlayerMovement playerMovement; // To lock input

    private int energyDrinkCount = 0;
    private int snackBarCount = 0;
    private int zenSodaCount = 0;
    private int totalCost = 0;

    private bool isUIActive = false;

    void Start()
    {
        vendingUI.SetActive(false); // Hide UI on load
        UpdateUI();
        SetCursorState(false); // Lock and hide on startup
        if (playerMovement != null)
        {
            playerMovement.inputLocked = false; // Ensure input is initially enabled
        }
    }

    // The Update() method for Tab key input is removed

    // Public method to activate the UI from another script
    public void ActivateUI()
    {
        if (!isUIActive) // Only activate if not already active
        {
            isUIActive = true;
            vendingUI.SetActive(true);
            SetCursorState(true); // Show cursor
            if (playerMovement != null)
            {
                playerMovement.inputLocked = true; // Lock input
            }
        }
    }

    // Public method to deactivate the UI from another script
    public void DeactivateUI()
    {
        if (isUIActive) // Only deactivate if currently active
        {
            isUIActive = false;
            vendingUI.SetActive(false);
            SetCursorState(false); // Lock cursor
            if (playerMovement != null)
            {
                playerMovement.inputLocked = false; // Unlock input
            }
        }
    }

    // Public method to toggle the UI (can be used if needed)
    public void ToggleUI()
    {
        isUIActive = !isUIActive;
        vendingUI.SetActive(isUIActive);
        SetCursorState(isUIActive);
        if (playerMovement != null)
        {
            playerMovement.inputLocked = isUIActive;
        }
    }

    void SetCursorState(bool isVisible)
    {
        Cursor.lockState = isVisible ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = isVisible;
    }

    public void AddEnergyDrink()
    {
        energyDrinkCount++;
        UpdateUI();
    }

    public void AddSnackBar()
    {
        snackBarCount++;
        UpdateUI();
    }

    public void AddZenSoda()
    {
        zenSodaCount++;
        UpdateUI();
    }

    public void ConfirmPurchase()
    {
        if (GameManager.Instance.paycheck >= totalCost)
        {
            GameManager.Instance.paycheck -= totalCost;
            Debug.Log("✅ Items purchased!");
            ResetCart();
        }
        else
        {
            Debug.LogWarning("🚫 Not enough paycheck.");
        }

        UpdateUI();
    }

    void ResetCart()
    {
        energyDrinkCount = 0;
        snackBarCount = 0;
        zenSodaCount = 0;
        totalCost = 0;
    }

    void UpdateUI()
    {
        totalCost = (energyDrinkCount * energyDrinkPrice) +
                    (snackBarCount * snackBarPrice) +
                    (zenSodaCount * zenSodaPrice);

        energyDrinkCountText.text = energyDrinkCount.ToString();
        snackBarCountText.text = snackBarCount.ToString();
        zenSodaCountText.text = zenSodaCount.ToString();
        totalText.text = $"Total: ${totalCost}";
        paycheckText.text = $"Paycheck: ${GameManager.Instance.paycheck}";
    }
}

