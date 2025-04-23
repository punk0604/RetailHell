using System.Collections;
using TMPro;
using UnityEngine;

public class BlinkingText : MonoBehaviour
{
    private TextMeshProUGUI text;
    private bool isVisible = true;

    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        StartCoroutine(BlinkText());
    }

    IEnumerator BlinkText()
    {
        while (true)
        {
            isVisible = !isVisible;
            text.enabled = isVisible;
            yield return new WaitForSeconds(0.7f); // Adjust speed of blinking
        }
    }
}