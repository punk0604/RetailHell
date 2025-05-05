using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ErrorSound : MonoBehaviour
{
    public static ErrorSound instance;

    public void play()
    {
        gameObject.GetComponent<AudioSource>().Play();
    }
}
