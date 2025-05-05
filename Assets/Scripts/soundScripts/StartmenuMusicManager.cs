using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class StartmenuMusicManager : MonoBehaviour
{
    public static StartmenuMusicManager instance;

    private void Awake() //make sure the music can play across scenes
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
