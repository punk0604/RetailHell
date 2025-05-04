using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int paycheck = 0;

    private void Awake()
    {
        if (Instance == null) { Instance = this; DontDestroyOnLoad(gameObject); }
        else Destroy(gameObject);
    }

    public int GetPaycheck() => paycheck;
    public void AddPaycheck(int amount) => paycheck += amount;
    public void ResetPaycheck() => paycheck = 0;

}
