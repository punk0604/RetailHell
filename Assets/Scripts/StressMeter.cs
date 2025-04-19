using UnityEngine;
using UnityEngine.UI;

public class StressMeter : MonoBehaviour
{
    public Slider stressSlider;
    public Image fillImage;

    public Color lowStressColor = Color.green;
    public Color midStressColor = Color.yellow;
    public Color highStressColor = Color.red;

    private float stressValue = 0f;
    private bool flashOutline = false;
    private float flashTimer = 0f;
    private float flashDuration = 0.5f;

    void Update()
    {
        UpdateStressVisual();
        if (flashOutline && stressValue >= stressSlider.maxValue)
            HandleFlash();
    }

    public void SetStress(float value)
    {
        stressValue = Mathf.Clamp(value, 0, stressSlider.maxValue);
        stressSlider.value = stressValue;

        flashOutline = (stressValue == stressSlider.maxValue);
    }

    void UpdateStressVisual()
    {
        if (stressValue < 50)
            fillImage.color = Color.Lerp(lowStressColor, midStressColor, stressValue / 50f);
        else
            fillImage.color = Color.Lerp(midStressColor, highStressColor, (stressValue - 50f) / 50f);
    }

    void HandleFlash()
    {
        flashTimer += Time.deltaTime;
        float t = Mathf.PingPong(flashTimer * 2f, 1f);
        Color flashColor = Color.Lerp(highStressColor, Color.white, t);
        fillImage.color = flashColor;
    }
}

