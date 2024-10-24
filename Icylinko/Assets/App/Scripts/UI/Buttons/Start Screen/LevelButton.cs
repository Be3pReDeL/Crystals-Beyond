using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    [SerializeField] private int _levelNumber;  // Номер уровня, устанавливаемый через инспектор
    [SerializeField] private TextController _levelText;  // Текст с номером уровня
    [SerializeField] private GameObject _completedIcon;  // Иконка завершенного уровня
    
    private int _realLevelNumber;
    private Button _button;

    private void Start()
    {
        _realLevelNumber = _levelNumber - 1;

        _button = GetComponent<Button>();

        // Устанавливаем текст с номером уровня
        _levelText.SetText(_levelNumber.ToString());

        // Проверяем, открыт ли уровень
        bool isUnlocked = PlayerPrefsController.IsLevelUnlocked(_realLevelNumber);
        _button.interactable = isUnlocked;

        // Показываем или скрываем иконку завершения уровня
        _completedIcon.SetActive(PlayerPrefsController.GetCompletedLevels(0) > _realLevelNumber);

        // Подписываемся на событие нажатия
        _button.onClick.AddListener(OnLevelButtonClick);
    }

    private void OnLevelButtonClick()
    {
        // Устанавливаем режим игры в "levels"
        PlayerPrefsController.SetGameMode(GameController.GameMode.levels);

        // Устанавливаем текущий уровень на основе номера, установленного в инспекторе
        PlayerPrefsController.SetLevel(_realLevelNumber);

        // Загружаем следующую сцену
        LoadSceneButton.LoadRelativeScene(1);
    }
}
