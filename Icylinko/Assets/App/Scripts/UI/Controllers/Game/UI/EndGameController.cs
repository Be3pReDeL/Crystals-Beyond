using UnityEngine;
using UnityEngine.UI;

public class EndGameController : MonoBehaviour
{
    public static EndGameController Instance { get; private set; }

    [SerializeField] private TextController _resultTextController;      // Контроллер текста для результата ("You win!" / "You loose!")
    [SerializeField] private TextController _levelTextController;      // Контроллер текста для уровня
    [SerializeField] private TextController _scoresTextController;     // Контроллер текста для очков

    [SerializeField] private Button _nextLevelButton;                  // Кнопка "Next Level"
    [SerializeField] private Button _mainMenuButton;                   // Кнопка "Main Menu"
    [SerializeField] private Button _restartButton;                    // Кнопка "Restart"

    [SerializeField] private ScreenController _HUDScreenController;

    [SerializeField] private GameObject _menuGameObject;             // Объект, содержащий текст уровня

    [SerializeField] private AudioClip _winAudioClip, _looseAudioClip;

    private AudioSource _audioSource;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);

        _audioSource = GetComponent<AudioSource>();
    }

    public void ShowEndGameScreen(bool isWinner, int score, bool isEndlessMode, int level = 0)
    {
        // Показываем экран конца игры
        _menuGameObject.SetActive(true);

        _HUDScreenController.CloseScreen();

        _audioSource.PlayOneShot(isWinner ? _winAudioClip : _looseAudioClip);

        VibrationController.Instance.Vibrate(VibrationController.VibrationType.heavy);

        // Устанавливаем текст результата
        _resultTextController.SetText(isWinner ? "You win!" : "You loose!");

        // Устанавливаем текст уровня, если это не Endless Mode
        if (isEndlessMode)
            _levelTextController.gameObject.SetActive(false);
        else
        {
            _levelTextController.gameObject.SetActive(true);
            _levelTextController.SetText($"Level: {level + 1}");
            // Отмечаем текущий уровень как завершенный
            PlayerPrefsController.SetLevelUnlocked(level + 1, true); // Открываем следующий уровень
            PlayerPrefsController.SetCompletedLevels(level); // Отмечаем текущий уровень как завершенный
        }

        PlayerPrefsController.SetPoints(PlayerPrefsController.GetPoints(0) + score);

        // Устанавливаем текст очков
        _scoresTextController.SetText($"Scores: {score}");

        // Активируем или деактивируем кнопку "Next Level"
        _nextLevelButton.gameObject.SetActive(isWinner && !isEndlessMode); // Кнопка доступна только при победе в режимах не Endless
    }

    public void HideEndGameScreen() => _menuGameObject.SetActive(false);  // Скрываем экран конца игры

    public void NextLevelButton()
    {
        int currentLevel = PlayerPrefsController.GetLevel(0);
        PlayerPrefsController.SetGameMode(GameController.GameMode.levels);
        PlayerPrefsController.SetLevel(currentLevel + 1); // Увеличиваем текущий уровень

        LoadSceneButton.LoadRelativeScene(0);
    }
}
