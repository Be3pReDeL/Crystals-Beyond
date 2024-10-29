using UnityEngine;
using UnityEngine.Audio;

public class SoundController : MonoBehaviour
{
    public static SoundController Instance { get; private set; }

    [SerializeField] private AudioMixer _audioMixer;

    private const float _musicEnabledVolume = -15f;  
    private const float _musicDisabledVolume = -80f; 
    private const float _masterEnabledVolume = 0f;  
    private const float _masterDisabledVolume = -80f; 

    private bool _isMusicEnabled;
    private bool _isSoundEnabled;

    private void Awake() 
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }

    private void Start()
    {
        ApplySavedSettings();
    }

    public void SetMusicEnabled(bool isEnabled)
    {
        _isMusicEnabled = isEnabled;  // Кэшируем текущее состояние
        _audioMixer.SetFloat("MusicVolume", isEnabled ? _musicEnabledVolume : _musicDisabledVolume);
        PlayerPrefsController.SetMusicEnabled(isEnabled);
    }

    public void SetSoundEnabled(bool isEnabled)
    {
        _isSoundEnabled = isEnabled;  // Кэшируем текущее состояние
        _audioMixer.SetFloat("MasterVolume", isEnabled ? _masterEnabledVolume : _masterDisabledVolume);
        PlayerPrefsController.SetSoundEnabled(isEnabled);
    }

    private void ApplySavedSettings()
    {
        // Применяем сохраненные настройки музыки
        _isMusicEnabled = PlayerPrefsController.IsMusicEnabled();
        SetMusicEnabled(_isMusicEnabled);  // Используем новый метод

        // Применяем сохраненные настройки звуков
        _isSoundEnabled = PlayerPrefsController.IsSoundEnabled();
        SetSoundEnabled(_isSoundEnabled);  // Используем новый метод
    }

    // Дополнительно, можно добавить метод для изменения громкости
    public void SetMasterVolume(float volume)
    {
        _audioMixer.SetFloat("MasterVolume", volume);
        // Сохраните текущее значение в PlayerPrefs, если это необходимо
    }
}
