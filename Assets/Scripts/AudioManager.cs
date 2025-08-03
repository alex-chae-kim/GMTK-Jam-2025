using UnityEngine;
using System;
using UnityEngine.Audio;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    public AudioMixerGroup soundFXGroup;
    public AudioMixerGroup musicGroup;
    public AudioMixerGroup narrationGroup;
    public SoundManager soundManager;

    public bool isNarrating;
    public List<Sound> randomNarrationSounds;
    bool[] alreadyPlayed;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // duplicate, destroy this scene copy
            return;
        }

        

        Instance = this;
        DontDestroyOnLoad(gameObject);

        float musicVol = PlayerPrefs.GetFloat("musicVolume", 1f);
        float sfxVol = PlayerPrefs.GetFloat("sfxVolume", 1f);
        float narrationVol = PlayerPrefs.GetFloat("narrationVolume", 1f);

        foreach (Sound s in soundManager.sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            if (s.music)
            {
                s.source.outputAudioMixerGroup = musicGroup;
            } else if (s.narration)
            {
                s.source.outputAudioMixerGroup = narrationGroup;
            } else
            {
                s.source.outputAudioMixerGroup = soundFXGroup;
            }
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.frequency = s.clip.frequency;
            s.samples = s.clip.samples;
            s.length = s.clip.length;
            s.source.loop = s.loop;

            if (s.music) s.source.volume = musicVol;
            else if (s.narration) s.source.volume = narrationVol;
            else s.source.volume = sfxVol;

            if (s.narration && s.randomNarration)
            {
                randomNarrationSounds.Add(s);
            }
        }
        alreadyPlayed = new bool[randomNarrationSounds.Count];
        for (int i = 0; i < randomNarrationSounds.Count; i++) { alreadyPlayed[i] = false; }
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

    public IEnumerator playWithFadeIn (string name)
    {
        Sound s = Array.Find(soundManager.sounds, sounds => sounds.name == name);
        if (s != null)
        {
            s.source.volume = 0f; // Start volume at 0 for fade-in
            s.source.Play();
            while (s.source.volume < s.volume)
            {
                s.source.volume += Time.deltaTime / 3f; // Adjust the fade-in duration as needed
                yield return null;
            }
            s.source.volume = s.volume; // Ensure it reaches the target volume
        }
    }

    public bool playRandomNarration()
    {
        int randomNumber = UnityEngine.Random.Range(0, randomNarrationSounds.Count);
        int totalSounds = 0;
        while (alreadyPlayed[randomNumber] && totalSounds < randomNarrationSounds.Count)
        {
            randomNumber++;
            totalSounds++;
            if (randomNumber >= randomNarrationSounds.Count)
            {
                randomNumber = 0;
            }
        }
        if (totalSounds >= randomNarrationSounds.Count)
        {
            return false; // All narration sounds have been played
        }
        if (isNarrating)
        {
            return false; // If narration is already playing, do not play another
        }
        else
        {
            Play(randomNarrationSounds[randomNumber].name);
            alreadyPlayed[randomNumber] = true; // Mark this sound as played
        }
        return true; // Successfully played a random narration sound
    }

    public IEnumerator playNarration(string name)
    {
        float originalMusicVol = 1;
        // fade out all music
        foreach (Sound s in soundManager.sounds)
        {
            if (s.music && s.source.isPlaying)
            {
                originalMusicVol = s.source.volume; // Store original volume
                float threshold = Math.Min(s.source.volume, 0.25f);
                Debug.Log($"Fading out music: {s.name} to threshold: {threshold}");
                yield return StartCoroutine(FadeOut(s, threshold)); // Fade out music
            }
        }
        //play narration
        Debug.Log($"Playing narration: {name}");
        Sound narrationSound = Array.Find(soundManager.sounds, sounds => sounds.name == name);
        Play(narrationSound.name);
        isNarrating = true;
        yield return new WaitUntil(() => !narrationSound.source.isPlaying);
        isNarrating = false;
        // fade in all music
        foreach (Sound s in soundManager.sounds)
        {
            if (s.music && s.source.isPlaying)
            {
                StartCoroutine(FadeIn(s, originalMusicVol)); // Fade out music
            }
        }
        
    }

    public void adjustVolume(float newVolume, string name)
    {
        Sound s = Array.Find(soundManager.sounds, sounds => sounds.name == name);
        if (s != null)
        {
            s.source.volume = newVolume;
        }
    }

    public void fadeOutHelper(String s, float threshold)
    {
        Sound soundToFade = Array.Find(soundManager.sounds, sound => sound.name == s);
        if (soundToFade.source.isPlaying)
        {
            StartCoroutine(FadeOut(soundToFade, threshold));
        }

    }

    public IEnumerator FadeOut(Sound s, float threshold)
    {
        while (s.source.volume > threshold)
        {
            s.source.volume -= Time.deltaTime / 2f;
            yield return null;
        }
        s.source.volume = threshold; // Ensure it reaches the target volume
        if (threshold <= 0)
        {
            s.source.Stop(); // Stop the sound if threshold is 0 or less
        }
    }

    public IEnumerator FadeIn(Sound s, float threshold)
    {
        while (s.source.volume < threshold)
        {
            s.source.volume += Time.deltaTime / 2f;
            yield return null;
        }
        s.source.volume = threshold; // Ensure it reaches the target volume
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
