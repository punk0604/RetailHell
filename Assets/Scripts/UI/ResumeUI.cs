using UnityEngine;
using TMPro;

public class ResumeUI : MonoBehaviour
{
    [Header("UI Elements")]
    public TMP_Text shiftsText;
    public TMP_Text paycheckText;

    private void Start()
    {
        gameObject.SetActive(false); // Panel hidden on start
    }

    public void ShowResume()
    {
        int shifts = GameManager.Instance?.GetSavedShifts() ?? 0;
        int paycheck = GameManager.Instance?.GetSavedPaycheck() ?? 0;

        shiftsText.text = "Shifts Survived: " + shifts;
        paycheckText.text = "Final Paycheck: $" + paycheck;

        gameObject.SetActive(true);
        Debug.Log("Resume shown.");
    }

    public void HideResume()
    {
        gameObject.SetActive(false);
        Debug.Log("Resume hidden.");
    }

    public void ClearResume()
    {
        GameManager.Instance?.ClearResumeStats();

        shiftsText.text = "Shifts Survived: 0";
        paycheckText.text = "Final Paycheck: $0";

        Debug.Log("Resume data cleared.");
    }
}