using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    [SerializeField] private int _levelNumber;
    [SerializeField] private TextController _levelText;
    [SerializeField] private GameObject _completedIcon;
    
    private int _realLevelNumber;
    private Button _button;

    private void Start()
    {
        _button = GetComponent<Button>();
        _realLevelNumber = _levelNumber - 1;
        
        // Инициализируем интерфейс
        InitializeUI();
        
        // Инициализируем состояние уровня
        InitializeLevelState();
        
        // Подписываемся на событие нажатия
        _button.onClick.AddListener(OnLevelButtonClick);
    }

    private void InitializeUI()
    {
        // Устанавливаем текст с номером уровня
        _levelText.SetText(_levelNumber.ToString());
    }

    private void InitializeLevelState()
    {
        bool isUnlocked = PlayerPrefsController.IsLevelUnlocked(_realLevelNumber);
        _button.interactable = isUnlocked;

        bool isCompleted = PlayerPrefsController.GetCompletedLevels(0) > _levelNumber;
        _completedIcon.SetActive(isCompleted);
    }

    private void OnLevelButtonClick()
    {
        // Настройка уровня и режима
        SetupLevel();
        
        // Загружаем сцену
        LoadSceneButton.LoadRelativeScene(1);
    }

    private void SetupLevel()
    {
        PlayerPrefsController.SetGameMode(GameController.GameMode.Levels);
        PlayerPrefsController.SetLevel(_realLevelNumber);
    }
}
