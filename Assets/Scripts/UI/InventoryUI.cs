using UnityEngine;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    public TMP_Text energyDrinkText;
    public TMP_Text snackBarText;
    public TMP_Text zenSodaText;

    private void Awake()
    {
        if (energyDrinkText == null)
            energyDrinkText = GameObject.Find("EnergyDrinkText")?.GetComponent<TMP_Text>();

        if (snackBarText == null)
            snackBarText = GameObject.Find("SnackBarText")?.GetComponent<TMP_Text>();

        if (zenSodaText == null)
            zenSodaText = GameObject.Find("ZenSodaText")?.GetComponent<TMP_Text>();
    }

    private void Update()
    {
        if (GameManager.Instance == null) return;

        if (energyDrinkText != null)
            energyDrinkText.text = $"[1]Energy Drinks: {GameManager.Instance.energyDrinksOwned}";

        if (snackBarText != null)
            snackBarText.text = $"[3]Snack Bars: {GameManager.Instance.snackBarsOwned}";

        if (zenSodaText != null)
            zenSodaText.text = $"[2]Zen Sodas: {GameManager.Instance.zenSodasOwned}";
    }
}

