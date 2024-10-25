using UnityEngine;
using UnityEngine.Audio;

public class SoundController : MonoBehaviour
{
    public static SoundController Instance { get; private set;}

    [SerializeField] private AudioMixer _audioMixer;

    private const float _musicEnabledVolume = -15f;  // Уровень громкости, когда звук включен
    private const float _musicDisabledVolume = -80f; // Уровень громкости, когда звук выключен
    private const float _masterEnabledVolume = 0f;  // Уровень громкости, когда звук включен
    private const float _masterDisabledVolume = -80f; // Уровень громкости, когда звук выключен

    private void Awake() 
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }

    private void Start()
    {
        // Применяем настройки при старте
        ApplySavedSettings();
    }

    public void SetMusicEnabled(bool isEnabled)
    {
        _audioMixer.SetFloat("MusicVolume", isEnabled ? _musicEnabledVolume : _musicDisabledVolume);
        PlayerPrefsController.SetMusicEnabled(isEnabled);
    }

    public void SetSoundEnabled(bool isEnabled)
    {
        _audioMixer.SetFloat("MasterVolume", isEnabled ? _masterEnabledVolume : _masterDisabledVolume);
        PlayerPrefsController.SetSoundEnabled(isEnabled);
    }

    private void ApplySavedSettings()
    {
        // Применяем сохраненные настройки музыки
        bool isMusicOn = PlayerPrefsController.IsMusicEnabled();
        _audioMixer.SetFloat("MusicVolume", isMusicOn ? _musicEnabledVolume : _musicDisabledVolume);

        // Применяем сохраненные настройки звуков
        bool isSoundOn = PlayerPrefsController.IsSoundEnabled();
        _audioMixer.SetFloat("MasterVolume", isSoundOn ? _masterEnabledVolume : _masterDisabledVolume);

        // Применяем сохраненные настройки вибрации
        // (функционал вибрации, если нужно, можно дополнить)
    }
}
