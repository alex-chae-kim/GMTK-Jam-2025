using System;
using System.Collections;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    //randomNarrationSounds will correspond to the respective random narration sounds in the assets
    //alreadyPlayed will indicate whether a narration has been played or not, in case of narration conflicts
    [SerializeField] AudioClip[] randomNarrationSounds = new AudioClip[9];
    [SerializeField] bool[] alreadyPlayed;
    //Store start game and end game narration here for organizational simplicity
    [SerializeField] AudioClip startGameNarration;
    [SerializeField] AudioClip endGameNarration;
    [SerializeField] AudioClip[] soundEffects = new AudioClip[4];
    public AudioManager audioManager;
    public AudioSource narrationSource;
    public bool isNarrating;

    [SerializeField] float timer;

    private void Start()
    {
        //Populate alreadyPlayed
        alreadyPlayed = new bool[randomNarrationSounds.Length];
        for(int i = 0; i < randomNarrationSounds.Length; i++) { alreadyPlayed[i] = false;}
    }

    //Requires input of an AudioClip correspinding to what audio you want played
    //and the index of the corresponding audio in the randomNarrationSounds array

    //Idk if any of this will actually be needed tho lmao
    public void playSound(AudioClip sound, int index)
    {
        //If there is not already a narration audio being played
        //AND the narration clip has not yet already been played,
        //play the audio.
        if (!isNarrating && !alreadyPlayed[index])
        {
            narrationSource.clip = sound;
            isNarrating = true;

            narrationSource.Play();
            StartCoroutine(WaitForAudioCompletion());
        }
    }

    //Waits for the narration to finish, then changes isNarrating back to false
    private IEnumerator WaitForAudioCompletion()
    {
        yield return new WaitUntil(() => !narrationSource.isPlaying);
        isNarrating = false;
    }
}
