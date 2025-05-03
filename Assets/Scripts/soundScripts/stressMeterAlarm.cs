using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stressMeterAlarm : MonoBehaviour
{
    AudioSource sound;
    void Start()
    {
        sound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
