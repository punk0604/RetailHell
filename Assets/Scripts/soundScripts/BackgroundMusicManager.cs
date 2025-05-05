using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusicManager : MonoBehaviour
{
    public AudioClip[] audioclips; //create an array of music options

    public AudioSource audioSource;
    private void Start()
    {
        //reference audio source in bgm object
        audioSource = GetComponent<AudioSource>();
    }
    private void Awake()
    {
        // play random music selection when level starts
        audioSource.clip = audioclips[Random.Range(0, audioclips.Length)];
        audioSource.Play();
    }
}
