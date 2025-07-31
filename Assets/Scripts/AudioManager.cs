using UnityEngine;
using System;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    public AudioMixerGroup soundFXGroup;
    public AudioMixerGroup musicGroup;
    public SoundManager soundManager;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // duplicate, destroy this scene copy
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        foreach (Sound s in soundManager.sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.outputAudioMixerGroup = s.music ? musicGroup : soundFXGroup;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.frequency = s.clip.frequency;
            s.samples = s.clip.samples;
            s.length = s.clip.length;
            s.source.loop = s.loop;
        }
    }

    /// <summary>
    /// Plays a sound by name
    /// </summary>
    /// <param name="name">The name of the sound to play</param>
    /// <param name="startTimeOffset">Sound will be played after startTimeOffset seconds have passed</param>
    /// <returns>The Sound object being played</returns>
    public Sound Play(string name, double startTimeOffset)
    {
        Sound s = Array.Find(soundManager.sounds, sounds => sounds.name == name);
        if (s == null)
        {
            return null;
        }
        s.source.PlayScheduled(AudioSettings.dspTime + startTimeOffset);
        return s;
    }

    /// <summary>
    /// Plays a sound by name
    /// </summary>
    /// <param name="name">The name of the sound to play</param>
    /// <returns>The Sound object being played</returns>
    public Sound Play(string name)
    {
        Sound s = Array.Find(soundManager.sounds, sounds => sounds.name == name);
        if (s == null)
        {
            return null;
        }
        s.source.Play();
        return s;
    }

    /// <summary>
    /// Stops a sound by name
    /// </summary>
    /// <param name="name">The name of the sound to stop</param>
    /// <returns></returns>
    public void Stop(string name)
    {
        Sound s = Array.Find(soundManager.sounds, sounds => sounds.name == name);
        if (s == null)
        {
            return;
        }
        s.source.Pause();
    }
}
