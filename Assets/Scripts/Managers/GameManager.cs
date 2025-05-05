using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int paycheck = 0;
    public int shiftsSurvived = 0;

    // Inventory
    public int energyDrinksOwned = 0;
    public int snackBarsOwned = 0;
    public int zenSodasOwned = 0;

    // PlayerPrefs keys
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

    // Paycheck
    public void AddPaycheck(int amount) => paycheck += amount;
    public int GetPaycheck() => paycheck;
    public void ResetPaycheck() => paycheck = 0;

    // Shift tracking
    public void IncrementShift() => shiftsSurvived++;
    public int GetShiftsSurvived() => shiftsSurvived;
    public void ResetShifts() => shiftsSurvived = 0;

    // Inventory Management
    public void AddToInventory(string item, int amount)
    {
        switch (item)
        {
            case "EnergyDrink":
                energyDrinksOwned += amount;
                break;
            case "SnackBar":
                snackBarsOwned += amount;
                break;
            case "ZenSoda":
                zenSodasOwned += amount;
                break;
        }
    }

    public void ResetInventory()
    {
        energyDrinksOwned = 0;
        snackBarsOwned = 0;
        zenSodasOwned = 0;
    }

    // Resume Save System
    public void SaveResumeStats()
    {
        PlayerPrefs.SetInt(ResumeShiftsKey, shiftsSurvived);
        PlayerPrefs.SetInt(ResumePaycheckKey, paycheck);
        PlayerPrefs.Save();
    }

    public int GetSavedShifts() => PlayerPrefs.GetInt(ResumeShiftsKey, 0);
    public int GetSavedPaycheck() => PlayerPrefs.GetInt(ResumePaycheckKey, 0);

    public void ClearResumeStats()
    {
        PlayerPrefs.DeleteKey(ResumeShiftsKey);
        PlayerPrefs.DeleteKey(ResumePaycheckKey);
        PlayerPrefs.Save();
    }
}


