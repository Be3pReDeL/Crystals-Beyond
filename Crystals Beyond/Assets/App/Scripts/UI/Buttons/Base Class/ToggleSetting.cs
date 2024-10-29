using UnityEngine;
using UnityEngine.UI;

public abstract class ToggleSetting : MonoBehaviour
{
    [SerializeField] private Sprite _onSprite;
    [SerializeField] private Sprite _offSprite;
    private Button _toggleButton;
    protected bool _isEnabled;
    
    protected abstract string PlayerPrefKey { get; }

    protected virtual void Awake() 
    {
        _toggleButton = GetComponent<Button>();
    }

    protected virtual void Start()
    {
        // Загружаем состояние и обновляем UI
        _isEnabled = LoadStateFromPlayerPrefs();
        UpdateUI();
        
        // Добавляем обработчик для изменения состояния
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
        // Обновляем визуальное представление в зависимости от состояния
        _toggleButton.image.sprite = _isEnabled ? _onSprite : _offSprite;
    }

    // Абстрактный метод для применения специфичной логики настройки
    protected abstract void ApplySetting();

    // Сохраняем состояние в PlayerPrefs через контроллер
    protected virtual void SaveStateToPlayerPrefs(bool state)
    {
        PlayerPrefsController.SetBool(PlayerPrefKey, state);
    }

    // Загружаем состояние из PlayerPrefs через контроллер
    protected virtual bool LoadStateFromPlayerPrefs()
    {
        return PlayerPrefsController.GetBool(PlayerPrefKey, true);
    }
}
