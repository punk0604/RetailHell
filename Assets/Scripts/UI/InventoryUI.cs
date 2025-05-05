using UnityEngine;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    //public static GameManager instance;

    public TMP_Text energyDrinkText;
    public TMP_Text snackBarText;
    public TMP_Text zenSodaText;

    private void Update()
    {
        energyDrinkText.text = $"[1]Energy Drinks: {GameManager.Instance.energyDrinksOwned}";
        snackBarText.text = $"[3]Snack Bars: {GameManager.Instance.snackBarsOwned}";
        zenSodaText.text = $"[2]Zen Sodas: {GameManager.Instance.zenSodasOwned}";
    }
}
