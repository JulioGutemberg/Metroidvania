using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemSound : MonoBehaviour
{
    private AudioSource audioSource;

    public AudioClip coindSound;
    public AudioClip jumpSound;
    public AudioClip hitSound;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();    
    }

    public void PlaySFX(AudioClip soundFX)
    {
        audioSource.PlayOneShot(soundFX);
    }
}
