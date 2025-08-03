using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class SettingsPopup : MonoBehaviour
{
    public Slider masterVolumeSlider;
    public Slider musicVolumeSlider;
    public Slider sfxVolumeSlider;
    public Slider narrationVolumeSlider;

    public Button closeButton;
    public GameManager gameManager;

    public float masterVolume = 1f;

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

        closeButton.onClick.AddListener(ClosePopup);

        masterVolumeSlider.value = masterVolume;
        musicVolumeSlider.value = GetAverageVolume(s => s.music);
        sfxVolumeSlider.value = GetAverageVolume(s => !s.music && !s.narration);
        narrationVolumeSlider.value = GetAverageVolume(s => s.narration);
    }

    float GetAverageVolume(System.Predicate<Sound> match)
    {
        var matchingSounds = AudioManager.Instance.soundManager.sounds.Where(s => match(s)).ToList();
        if (matchingSounds.Count == 0) return 1f;
        return matchingSounds.Average(s => s.source.volume);
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
        foreach (Sound s in AudioManager.Instance.soundManager.sounds)
        {
            if (s.music)
                s.source.volume = value;
        }
        PlayerPrefs.SetFloat("musicVolume", value);
    }

    public void ChangeSFXVolume(float value)
    {
        foreach (Sound s in AudioManager.Instance.soundManager.sounds)
        {
            if (!s.music && !s.narration)
                s.source.volume = value;
        }
        PlayerPrefs.SetFloat("sfxVolume", value);
    }

    public void ChangeNarrationVolume(float value)
    {
        foreach (Sound s in AudioManager.Instance.soundManager.sounds)
        {
            if (s.narration)
                s.source.volume = value;
        }
        PlayerPrefs.SetFloat("narrationVolume", value);
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
