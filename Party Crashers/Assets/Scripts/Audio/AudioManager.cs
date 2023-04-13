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
    public float musicVolume;

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
            if (s.name != "Hyena_Attack" || s.name != "Bird_Fire")
            {
                s.source = gameObject.AddComponent<AudioSource>();
            }
            s.source.clip = s.clip;
            s.source.outputAudioMixerGroup = masterMixer;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.panStereo = s.panStereo;
            s.source.spatialBlend = s.spacialBlend;
        }
    }

    #region Sound Controls

    public AudioSource SoundSourceOnObject(string name, GameObject obj)
    {
        Sound s = Array.Find(Sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning(name + ": source not found");
            return null;
        }
        s.source = obj.GetComponent<AudioSource>();
        return s.source;
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

    public void AddSound(string soundName, GameObject obj)
    {
        Sound s = Array.Find(Sounds, sound => sound.name == soundName);
        if (s == null)
        {
            Debug.LogWarning(soundName + ": audio not found");
            return;
        }

        s.source = obj.AddComponent<AudioSource>();
        s.source.clip = s.clip;
        s.source.outputAudioMixerGroup = masterMixer;
        s.source.volume = s.volume;
        s.source.pitch = s.pitch;
        s.source.loop = s.loop;
        s.source.panStereo = s.panStereo;
        s.source.spatialBlend = s.spacialBlend;
        
        print("sound '" + soundName + "' added to " + obj.name);
    }

    public void PlayAddedSound(string soundName, GameObject obj)
    {
        Sound s = Array.Find(Sounds, sound => sound.name == soundName);
        if (s == null)
        {
            Debug.LogWarning(soundName + ": audio not found");
            return;
        }
        s.source = obj.GetComponent<AudioSource>();
        if (!s.source.isPlaying)
        {
            s.source.Play();
        }
    }

    public void Stop(string name)
    {
        Sound s = Array.Find(Sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning(name + ": audio not found");
            return;
        }
        s.source.Stop();
    }

    public void Pause(string name)
    {
        Sound s = Array.Find(Sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning(name + ": audio not found");
            return;
        }
        s.source.Pause();
    }

    public void UnPause(string name)
    {
        Sound s = Array.Find(Sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning(name + ": audio not found");
            return;
        }
        s.source.UnPause();
    }


    public float ClipLength(string name)
    {
        Sound s = Array.Find(Sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning(name + ": audio not found");
            return 0;
        }
        print(s.source.clip.length);
        return s.source.clip.length;
    }

    /// <summary>
    /// Specifically for music, this function disables the volume of a clip 
    /// as soon as it plays
    /// </summary>
    /// <param name="name"></param>
    public void PlayMuted(string name)
    {
        Sound s = Array.Find(Sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning(name + ": audio not found");
            return;
        }
        s.source.Play();
        s.source.volume = 0.0f;
    }

        /// <summary>
        /// Specifically for music, this function disables the volume of a clip
        /// </summary>
        /// <param name="name"></param>
        public void Mute(string name)
    {
        Sound s = Array.Find(Sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning(name + ": audio not found");
            return;
        }
        s.source.volume = 0.0f;
    }

    /// <summary>
    /// Specifically for music, this function enables the volume of a clip
    /// </summary>
    /// <param name="name"></param>
    public void Unmute(string name)
    {
        Sound s = Array.Find(Sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning(name + ": audio not found");
            return;
        }
        s.source.volume = musicVolume;
    }

    #endregion

    #region Song Functions

    public void SwitchMusic(int musicTrack)
    {
        MuteAllInstruments();
        Stop("Tutorial_Music");
        Stop("Pause_Music");
        switch (musicTrack)
        {
            // TUTORIAL
            case 1:
                print("Tutorial music");
                Play("Tutorial_Music");
                PlayMuted("Drum_Set");
                break;
            case 2:
                print("Move 1 music");
                MusicMoveOne();
                break;
            case 3:
                print("fight 1 music");
                MusicFightOne();
                break;
            case 4:
                print("move 2 music");
                MusicMoveTwo();
                break;
            case 5:
                print("fight 2 music");
                MusicFightTwo();
                break;
            case 6:
                print("move 3 music");
                MusicMoveThree();
                break;
            case 7:
                print("fight 3 music");
                MusicFightThree();
                break;
            case 8:
                print("move 4 music");
                MusicMoveFour();
                break;
            case 9:
                print("boss music");
                Play("Boss_Music");
                break;
            // PAUSED
            case 10:
                MuteAllInstruments();
                print("pause music");
                Play("Pause_Music");
                Stop("Tutorial_Music");
                Stop("Boss_Music");
                // 
                break;
            case 11:
                print("Victory music");
                //Play("Victory_Music");
                // 
                break;
            case 12:
                print("End Game music");
                MuteAllInstruments();
                Play("End_Music");
                // 
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Play the components of EVERY SONG, with only the components for move 1 audible
    /// </summary>
    public void MusicMoveOne()
    {
        // Play the components of move song 1
        Stop("Tutorial_Song");
        Play("Toy_Piano");
        Play("Hand_Clap");
        Play("Foot_Stomp");
        Play("Ukulele");
        // Play and mute the rest of the components immediately
        PlayMuted("Harm_Guitar");
        PlayMuted("Bass_Guitar");
        PlayMuted("Lead_Guitar");
        PlayMuted("Drum_Set");
        // unmute other instruments in case of pause
        Unmute("Toy_Piano");
        Unmute("Hand_Clap");
        Unmute("Foot_Stomp");
        Unmute("Ukulele");
    }

    /// <summary>
    /// Play the components of the first fight song
    /// </summary>
    public void MusicFightOne()
    {
        // unmute the missing instruments
        Unmute("Bass_Guitar");
        Unmute("Harm_Guitar");
        // unmute other instruments in case of pause
        Unmute("Toy_Piano");
        Unmute("Hand_Clap");
        Unmute("Foot_Stomp");
        Unmute("Ukulele");
    }

    /// <summary>
    /// Play the components of the second move song
    /// </summary>
    public void MusicMoveTwo()
    {
        // mute unecessary intrument
        Mute("Toy_Piano");
        // unmute other instruments in case of pause
        Unmute("Bass_Guitar");
        Unmute("Harm_Guitar");
        Unmute("Hand_Clap");
        Unmute("Foot_Stomp");
        Unmute("Ukulele");
    }

    /// <summary>
    /// Play the components of the second fight song
    /// </summary>
    public void MusicFightTwo()
    {
        // unmute the missing instruments
        Unmute("Drum_Set");
        // mute unecessary intruments
        Stop("Hand_Clap");
        Stop("Foot_Stomp");
        // unmute other instruments in case of pause
        Unmute("Toy_Piano");
        Unmute("Ukulele");
        Unmute("Bass_Guitar");
        Unmute("Harm_Guitar");
    }

    /// <summary>
    /// Play the components of the third move song
    /// </summary>
    public void MusicMoveThree()
    {
        // mute unecessary intruments
        Stop("Toy_Piano");
        // unmute other instruments in case of pause
        Unmute("Ukulele");
        Unmute("Bass_Guitar");
        Unmute("Harm_Guitar");
        Unmute("Drum_Set");

    }

    /// <summary>
    /// Play the components of the third fight song
    /// </summary>
    public void MusicFightThree()
    {
        // unmute the missing instrument
        Unmute("Lead_Guitar");
        // unmute other instruments in case of pause
        Unmute("Ukulele");
        Unmute("Bass_Guitar");
        Unmute("Harm_Guitar");
        Unmute("Drum_Set");
    }

    /// <summary>
    /// Play the components of the fourth move song
    /// </summary>
    public void MusicMoveFour()
    {
        // Stop other instruments since the boss song is next
        Stop("Lead_Guitar");
        Stop("Harm_Guitar");
        Stop("Bass_Guitar");
        Stop("Ukulele");
        // unmute the drums in case of pause
        Unmute("Drum_Set");
    }

    /// <summary>
    /// Mute all instruments when the game is paused
    /// </summary>
    public void MuteAllInstruments()
    {
        Mute("Toy_Piano");
        Mute("Lead_Guitar");
        Mute("Drum_Set");
        Mute("Bass_Guitar");
        Mute("Harm_Guitar");
        Mute("Hand_Clap");
        Mute("Foot_Stomp");
        Mute("Ukulele");
    }
    #endregion
}
