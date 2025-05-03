using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class ProgressBar : MonoBehaviour
{
    [Header("Title Setting")]
    public string Title;
    public Color TitleColor;
    public Font TitleFont;
    public int TitleFontSize = 10;

    [Header("Bar Setting")]
    public Color BarColor;
    public Color BarBackGroundColor;
    public Sprite BarBackGroundSprite;
    [Range(1f, 100f)]
    public float Alert = 75f;
    public Color BarAlertColor;

    [Header("Flashing Effect")]
    public bool enableFlashing = true;
    public float flashSpeed = 2f;

    private Image bar, barBackground;
    private Text txtTitle;
    private bool isFlashing = false;
    private float flashTimer = 0f;
    private bool isGameOver = false;
    private float barValue;

    public float BarValue
    {
        get { return barValue; }
        set
        {
            value = Mathf.Clamp(value, 0, 100);
            barValue = value;
            UpdateValue(barValue);
        }
    }

    private void Awake()
    {
        bar = transform.Find("Bar").GetComponent<Image>();
        barBackground = transform.Find("BarBackground").GetComponent<Image>();
        txtTitle = transform.Find("Text").GetComponent<Text>();
    }

    private void Start()
    {
        txtTitle.text = Title;
        txtTitle.color = TitleColor;
        txtTitle.font = TitleFont;
        txtTitle.fontSize = TitleFontSize;

        bar.color = BarColor;
        barBackground.color = BarBackGroundColor;
        barBackground.sprite = BarBackGroundSprite;

        UpdateValue(barValue);
    }

    void UpdateValue(float val)
    {
        bar.fillAmount = val / 100;
        val = (float)System.Math.Round(val, 0);
        txtTitle.text = Title + " " + val + "%";

        if (Alert <= val)
        {
            bar.color = BarAlertColor;
        }
        else
        {
            bar.color = BarColor;
        }
    }

    private void Update()
    {
        if (!Application.isPlaying)
        {
            UpdateValue(50);
            txtTitle.color = TitleColor;
            txtTitle.font = TitleFont;
            txtTitle.fontSize = TitleFontSize;

            bar.color = BarColor;
            barBackground.color = BarBackGroundColor;
            barBackground.sprite = BarBackGroundSprite;
        }
        else
        {
            // Flashing when 75% or more
            if (enableFlashing && barValue >= 75f)
            {
                isFlashing = true;
                GameObject.Find("stressMeterAlarm").GetComponent<AudioSource>().Play();
            }
            else
            {
                isFlashing = false;
                bar.color = (Alert >= barValue) ? BarAlertColor : BarColor;
            }

            if (isFlashing)
            {
                flashTimer += Time.deltaTime * flashSpeed;
                float lerp = Mathf.PingPong(flashTimer, 1f);
                Color flashColor = Color.Lerp(BarAlertColor, new Color(BarAlertColor.r, BarAlertColor.g, BarAlertColor.b, 0.3f), lerp);
                bar.color = flashColor;
            }

            // Game Over trigger
            if (!isGameOver && barValue >= 100f)
            {
                isGameOver = true;
                GameObject.Find("stressMeterAlarm").GetComponent<AudioSource>().Stop();
                Debug.Log("Game Over");
            }
        }
    }
}


