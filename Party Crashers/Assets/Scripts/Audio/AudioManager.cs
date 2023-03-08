using System;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public Sound[] Sounds;
    
    // Start is called before the first frame update
    void Awake()
    {
        foreach (Sound s in Sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Play(string name)
    {
        Sound s = Array.Find(Sounds, sound => sound.name == name);
        s.source.Play();
    }
}
