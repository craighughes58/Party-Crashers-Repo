/*****************************************************************************
    Brackeys Audio Manager
    Tutorial video: https://youtu.be/6OT43pvUyfY

    Author: Caden Sheahan
    Date: 3/1/23
    Description: Creates the settings for the audio sources added to each sound
    within the AudioManager.
******************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class Sound
{
    public AudioClip clip;

    public string name;

    [Range(0, 1)]
    public float volume;
    [Range(0.1f, 3)]
    public float pitch;
    public bool loop;

    [Range(-1, 1)]
    public float panStereo;
    [Range(0, 1)]
    public float spacialBlend;
    public int minDistance;
    public int maxDistance;

    [HideInInspector]
    public AudioSource source;
}
