using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stressMeterAlarm : MonoBehaviour
{
    AudioSource sound;
    public bool stressHigh;
    void Start()
    {
        sound = GetComponent<AudioSource>();
    }

    public bool stressCheck
    {
        get{ return stressHigh;}
        set
        {
            if (value == stressHigh)
            {
                return;
            }
            stressHigh = value;
            if(stressHigh)
            {
                sound.Play();
            }
            else
            {
                sound.Stop();
            }
        }
    }
}
