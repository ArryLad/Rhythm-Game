using UnityEngine.Audio;
using UnityEngine;

//Brackeys (2017) Introduction to AUDIO in Unity [Online Video] [Access Date: 9th May 2022] ]https://youtu.be/6OT43pvUyfY

[System.Serializable]
public class Sound
{
    //Each sound has a name, audio clip, volume from 0-1 and an audio source
    public string name;

    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume;

    [HideInInspector]
    public AudioSource source;
}
