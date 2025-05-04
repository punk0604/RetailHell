using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int paycheck = 0;
    public int shiftsSurvived = 0;

    private const string ResumeShiftsKey = "Resume_Shifts";
    private const string ResumePaycheckKey = "Resume_Paycheck";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    public void AddPaycheck(int amount) => paycheck += amount;
    public int GetPaycheck() => paycheck;

    public void ResetPaycheck() => paycheck = 0;

    public void IncrementShift() => shiftsSurvived++;
    public int GetShiftsSurvived() => shiftsSurvived;
    public void ResetShifts() => shiftsSurvived = 0;

    // Save current game results to resume data
    public void SaveResumeStats()
    {
        PlayerPrefs.SetInt(ResumeShiftsKey, shiftsSurvived);
        PlayerPrefs.SetInt(ResumePaycheckKey, paycheck);
        PlayerPrefs.Save();
    }

    // Access saved resume data
    public int GetSavedShifts() => PlayerPrefs.GetInt(ResumeShiftsKey, 0);
    public int GetSavedPaycheck() => PlayerPrefs.GetInt(ResumePaycheckKey, 0);

    // Reset saved resume data (optional for testing)
    public void ClearResumeStats()
    {
        PlayerPrefs.DeleteKey(ResumeShiftsKey);
        PlayerPrefs.DeleteKey(ResumePaycheckKey);
    }
}


