using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class SettingsPopup : MonoBehaviour
{
    public Slider masterVolumeSlider;
    public Slider musicVolumeSlider;
    public Slider sfxVolumeSlider;
    public Slider narrationVolumeSlider;

    public Button closeButton;
    public GameManager gameManager;

    public AudioSource musicSource;
    public AudioSource sfxSource;
    public AudioSource narrationSource;

    public float masterVolume = 1f;
    public bool isPlayScene;

    void Awake()
    {
        if (gameManager == null)
            gameManager = FindObjectOfType<GameManager>();
    }

    private void Start()
    {
        masterVolumeSlider.onValueChanged.AddListener(ChangeMasterVolume);
        musicVolumeSlider.onValueChanged.AddListener(ChangeMusicVolume);
        sfxVolumeSlider.onValueChanged.AddListener(ChangeSFXVolume);
        narrationVolumeSlider.onValueChanged.AddListener(ChangeNarrationVolume);

        closeButton.onClick.AddListener(() => {ClosePopup();});

        masterVolumeSlider.value = masterVolume;
        musicVolumeSlider.value = musicSource.volume;
        sfxVolumeSlider.value = sfxSource.volume;
        narrationVolumeSlider.value = narrationSource.volume;
    }

    public void ChangeMasterVolume(float value)
    {
        masterVolume = value;
        ChangeMusicVolume(value);
        ChangeSFXVolume(value);
        ChangeNarrationVolume(value);
    }

    public void ChangeMusicVolume(float value)
    {
        AudioListener.volume = value;
        if (musicSource != null) musicSource.volume = value;
        PlayerPrefs.SetFloat("volume", value);
    }

    public void ChangeSFXVolume(float value)
    {
        AudioListener.volume = value;
        if (sfxSource != null) sfxSource.volume = value;
        PlayerPrefs.SetFloat("volume", value);
    }

    public void ChangeNarrationVolume(float value)
    {
        AudioListener.volume = value;
        if (narrationSource != null) narrationSource.volume = value;
        PlayerPrefs.SetFloat("volume", value);
    }

    public void OpenPopup()
    {
        gameObject.SetActive(true);
        gameManager.pauseGame();
        Debug.Log("Opening settings");
    }

    public void ClosePopup()
    {
        gameObject.SetActive(false);
        gameManager.resumeGame();
        Debug.Log("Closing settings");
    }
}
