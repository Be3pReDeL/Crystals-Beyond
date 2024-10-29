using UnityEngine;
using UnityEngine.UI;

public class EndGameController : MonoBehaviour
{
    public static EndGameController Instance { get; private set; }

    [SerializeField] private TextController _resultTextController;
    [SerializeField] private TextController _levelTextController;
    [SerializeField] private TextController _scoresTextController;

    [SerializeField] private Button _nextLevelButton;
    [SerializeField] private Button _mainMenuButton;
    [SerializeField] private Button _restartButton;

    [SerializeField] private ScreenController _HUDScreenController;
    [SerializeField] private GameObject _menuGameObject;

    [SerializeField] private AudioClip _winAudioClip, _looseAudioClip;

    private AudioSource _audioSource;

    private const string WIN_TEXT = "You win!";
    private const string LOOSE_TEXT = "You lose!";
    private const string LEVEL_TEXT = "Level: {0}";
    private const string SCORES_TEXT = "Scores: {0}";

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
        
        _audioSource = GetComponent<AudioSource>();
    }

    public void ShowEndGameScreen(bool isWinner, int score, bool isEndlessMode, int level = 0)
    {
        _menuGameObject.SetActive(true);
        _HUDScreenController.CloseScreen();

        PlayEndGameSound(isWinner);
        VibrateEndGame();

        SetResultText(isWinner);
        SetScoreText(score);
        ConfigureLevelText(isEndlessMode, level);

        PlayerPrefsController.SetPoints(PlayerPrefsController.GetPoints(0) + score);

        _nextLevelButton.gameObject.SetActive(isWinner && !isEndlessMode);
    }

    private void PlayEndGameSound(bool isWinner)
    {
        _audioSource.PlayOneShot(isWinner ? _winAudioClip : _looseAudioClip);
    }

    private void VibrateEndGame()
    {
        VibrationController.Instance?.Vibrate(VibrationController.VibrationType.heavy);
    }

    private void SetResultText(bool isWinner)
    {
        _resultTextController.SetText(isWinner ? WIN_TEXT : LOOSE_TEXT);
    }

    private void SetScoreText(int score)
    {
        _scoresTextController.SetText(string.Format(SCORES_TEXT, score));
    }

    private void ConfigureLevelText(bool isEndlessMode, int level)
    {
        if (isEndlessMode)
        {
            _levelTextController.gameObject.SetActive(false);
        }
        else
        {
            _levelTextController.gameObject.SetActive(true);
            _levelTextController.SetText(string.Format(LEVEL_TEXT, level + 1));
            PlayerPrefsController.SetLevelUnlocked(level + 1, true);
            PlayerPrefsController.SetCompletedLevels(level);
        }
    }

    public void NextLevelButton()
    {
        PlayerPrefsController.SetGameMode(GameController.GameMode.Levels);
        PlayerPrefsController.SetLevel(PlayerPrefsController.GetLevel(0) + 1);
        LoadSceneButton.LoadRelativeScene(0);
    }

    public void HideEndGameScreen() => _menuGameObject.SetActive(false);
}
