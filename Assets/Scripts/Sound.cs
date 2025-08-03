using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name;

    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume;
    [Range(0.1f, 3f)]
    public float pitch;
    public bool music;
    public bool narration;
    public bool randomNarration;
    public bool loop;


    [HideInInspector] public AudioSource source;
    [HideInInspector] public float length;
    [HideInInspector] public float frequency;
    [HideInInspector] public float samples;
}
