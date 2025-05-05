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

    private int energyDrinkCount = 0;
    private int snackBarCount = 0;
    private int zenSodaCount = 0;

    private int totalCost = 0;

    void Start()
    {
        UpdateUI();
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
            Debug.Log("Items purchased!");
            ResetCart();
        }
        else
        {
            Debug.LogWarning("Not enough paycheck to purchase items.");
        }

        UpdateUI();
    }

    private void ResetCart()
    {
        energyDrinkCount = 0;
        snackBarCount = 0;
        zenSodaCount = 0;
        totalCost = 0;
    }

    private void UpdateUI()
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
