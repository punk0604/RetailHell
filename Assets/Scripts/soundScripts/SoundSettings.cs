using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class SoundSettings : MonoBehaviour
{
    [SerializeField] Slider soundSlider;
    [SerializeField] AudioMixer masterMixer;
    // Start is called before the first frame update
    public float defaultvolume;
    private void Start()
    {
        SetVolume(PlayerPrefs.GetFloat("SavedMasterVolume", 100));
        defaultvolume = 100;
        SetVolume(defaultvolume);
    }

    public void SetVolume(float _value)
    {
        if(_value < 1)
        {
            _value = .001f;
        }

        RefreshSlider(_value);
        Debug.Log("volume set to " +  _value);
        PlayerPrefs.SetFloat("SavedMasterVolume", _value);
        masterMixer.SetFloat("MasterVolume", Mathf.Log10(_value / 100) * 20f);
    }

    // Update is called once per frame
    public void SetVolumeFromSlider()
    {
        SetVolume(soundSlider.value);
    }

    public void RefreshSlider(float _value)
    {
        soundSlider.value = _value;
    }
}
