using UnityEngine;
using TMPro;

public class StatsMenuUI : MonoBehaviour
{
    public TMP_Text paycheckText;
    public TMP_Text shiftsText;

    public void Refresh()
    {
        if (GameManager.Instance != null)
        {
            paycheckText.text = "Paycheck: $" + GameManager.Instance.GetPaycheck();
            shiftsText.text = "Shifts Survived: " + GameManager.Instance.GetShiftsSurvived();
        }
    }

    public void ToggleVisibility()
    {
        bool isActive = gameObject.activeSelf;
        gameObject.SetActive(!isActive);
        if (!isActive) Refresh();
    }
}

