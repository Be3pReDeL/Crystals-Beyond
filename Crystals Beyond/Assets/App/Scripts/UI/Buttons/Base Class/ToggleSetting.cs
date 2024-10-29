using UnityEngine;
using UnityEngine.UI;

public abstract class ToggleSetting : MonoBehaviour
{
    [SerializeField] protected Sprite _onSprite;
    [SerializeField] protected Sprite _offSprite;
    protected Button _toggleButton;
    protected bool _isEnabled;
    
    // Для каждого конкретного класса нужно переопределить эти ключи
    protected abstract string PlayerPrefKey { get; }

    protected virtual void Awake() 
    {
        _toggleButton = GetComponent<Button>();
    }

    protected virtual void Start()
    {
        // Загружаем сохраненное состояние
        _isEnabled = LoadStateFromPlayerPrefs();
        UpdateUI();
        _toggleButton.onClick.AddListener(ToggleState);
    }

    private void ToggleState()
    {
        _isEnabled = !_isEnabled;
        SaveStateToPlayerPrefs(_isEnabled);
        UpdateUI();
        ApplySetting();
    }

    protected virtual void UpdateUI()
    {
        _toggleButton.image.sprite = _isEnabled ? _onSprite : _offSprite;
    }

    // Применение настройки через SoundController
    protected abstract void ApplySetting();

    // Сохраняем текущее состояние в PlayerPrefs
    protected virtual void SaveStateToPlayerPrefs(bool state)
    {
        PlayerPrefsController.SetBool(PlayerPrefKey, state);
    }

    // Загружаем сохраненное состояние из PlayerPrefs
    protected virtual bool LoadStateFromPlayerPrefs()
    {
        return PlayerPrefsController.GetBool(PlayerPrefKey, true);
    }
}
