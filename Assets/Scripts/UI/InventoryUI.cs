using UnityEngine;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    public TMP_Text energyDrinkText;
    public TMP_Text snackBarText;
    public TMP_Text zenSodaText;

    private void Update()
    {
        energyDrinkText.text = $"Energy Drinks: {GameManager.Instance.energyDrinksOwned}";
        snackBarText.text = $"Snack Bars: {GameManager.Instance.snackBarsOwned}";
        zenSodaText.text = $"Zen Sodas: {GameManager.Instance.zenSodasOwned}";
    }
}
