using UnityEngine;
using UnityEngine.Audio;

public class SoundController : MonoBehaviour
{
    public static SoundController Instance { get; private set;}

    [SerializeField] private AudioMixer _audioMixer;

    private const float _enabledVolume = 0f;  // Уровень громкости, когда звук включен
    private const float _disabledVolume = -80f; // Уровень громкости, когда звук выключен

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
        _audioMixer.SetFloat("MusicVolume", isEnabled ? _enabledVolume : _disabledVolume);
        PlayerPrefsController.SetMusicEnabled(isEnabled);
    }

    public void SetSoundEnabled(bool isEnabled)
    {
        _audioMixer.SetFloat("MasterVolume", isEnabled ? _enabledVolume : _disabledVolume);
        PlayerPrefsController.SetSoundEnabled(isEnabled);
    }

    private void ApplySavedSettings()
    {
        // Применяем сохраненные настройки музыки
        bool isMusicOn = PlayerPrefsController.IsMusicEnabled();
        _audioMixer.SetFloat("MusicVolume", isMusicOn ? _enabledVolume : _disabledVolume);

        // Применяем сохраненные настройки звуков
        bool isSoundOn = PlayerPrefsController.IsSoundEnabled();
        _audioMixer.SetFloat("MasterVolume", isSoundOn ? _enabledVolume : _disabledVolume);

        // Применяем сохраненные настройки вибрации
        // (функционал вибрации, если нужно, можно дополнить)
    }
}
