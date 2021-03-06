using UnityEngine.Audio;
using System;
using UnityEngine;

//Brackeys (2017) Introduction to AUDIO in Unity [Online Video] [Access Date: 9th May 2022] ]https://youtu.be/6OT43pvUyfY

public class AudioManager : MonoBehaviour
{
    //list of sounds
    public Sound[] sounds;

    void Awake()
    {
        //Every element in sounds in Sound give it an audioScource
        //and get its sound clip and volume
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            
        }
    }

    //Function which finds the sound in Sound[] by name and plays that sound
    public void Play(string name)
    {
        Sound s= Array.Find(sounds, sound => sound.name == name);
        s.source.Play();
    }
}
