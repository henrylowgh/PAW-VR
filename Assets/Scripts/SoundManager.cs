using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    public List<AudioClip> audioClips;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayClip(int audioIndex)
    {
        /* Debugging */
        if (audioSource == null)
        {
            Debug.LogError("AudioSource not found!");
            return;
        }

        if (audioIndex < 0 || audioIndex >= audioClips.Count)
        {
            Debug.LogError("Audio index out of range!");
            return;
        }

        Debug.Log("accessed");
        audioSource.PlayOneShot(audioClips[audioIndex]);
    }
}
