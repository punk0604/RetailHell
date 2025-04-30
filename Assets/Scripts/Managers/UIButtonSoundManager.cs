using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIButtonSoundManager : MonoBehaviour
{
    public static UIButtonSoundManager instance;
    private void Awake() //make sure the sound can play across scenes
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}
