using UnityEngine;
using TMPro;

public class InteractionPromptUI : MonoBehaviour
{
    public static InteractionPromptUI Instance;

    public GameObject promptRoot;      // Parent container (can be the text itself)
    public TMP_Text promptText;        // The TMP text element

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        Hide(); // Start hidden
    }

    public void Show(string message)
    {
        promptText.text = message;
        promptRoot.SetActive(true);
    }

    public void Hide()
    {
        promptRoot.SetActive(false);
    }
}

