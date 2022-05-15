using UnityEngine.Audio;
using UnityEngine;

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
