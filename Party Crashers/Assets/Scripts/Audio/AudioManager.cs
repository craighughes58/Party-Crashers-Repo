/*****************************************************************************
    Brackeys Audio Manager
    Tutorial video: https://youtu.be/6OT43pvUyfY

    Author: Caden Sheahan
    Date: 3/8/23
    Description: Creates an array of all sound effects defined by "Sound" script
    and adds an audio source to each of them in the AudioManager game object.
    
    Use this line to play any sound anywhere, if you have the name set in 
    the inspector:
    
    FindObjectOfType<AudioManager>().Play("[INSERT_NAME_FROM_INSPECTOR]");
 *****************************************************************************/
using System;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public Sound[] Sounds;
    public AudioMixerGroup masterMixer;
    private int movePoint = 0;

    public static AudioManager instance;
    
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        
        DontDestroyOnLoad(gameObject);
        
        foreach (Sound s in Sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.outputAudioMixerGroup = masterMixer;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.panStereo = s.panStereo;
            s.source.spatialBlend = s.spacialBlend;
        }
    }

    private void Start()
    {
        Play("Music_Tutorial");
    }

    public void Play(string name)
    {
        Sound s = Array.Find(Sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning(name + ": audio not found");
            return;
        }
        s.source.Play();
    }

    // DO NOT DELETE
    //public void SwitchMusic(string currentSong, string nextSong)
    //{
    //    Sound s1 = Array.Find(Sounds, sound => sound.name == currentSong);
    //    Sound s2 = Array.Find(Sounds, sound => sound.name == nextSong);
    //    s1.source.time = s1.startTime;
    //    s2.source.time = s1.endTime;
    //    s1.source.SetScheduledEndTime(AudioSettings.dspTime + (s1.endTime - s1.source.time));
    //    s2.source.SetScheduledStartTime(AudioSettings.dspTime + ()
    //}
    public void SwitchMovingMusic()
    {

    }

    public void SwitchMusic(int musicTrack)
    {
        switch (musicTrack)
        {
            // TUTORIAL
            case 1:
                print("TURORIAL");
                //Play( int "musicTrack")
                break;
            case 2:
                print(" music 2");
                break;
            case 3:
                print(" music 3");
                break;
            case 4:
                print(" music 4");
                break;
            case 5:
                print(" music 5");
                break;
            case 6:
                print(" music 6");
                break;
            case 7:
                print(" music 7");
                break;
            case 8:
                print(" music 8");
                break;
            // PAUSED
            case 9:
                print("pause music");
                // 
                break;
            default:
                break;
        }
    }
}
