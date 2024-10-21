using UnityEngine;

public class HUDController : MonoBehaviour
{
    public static HUDController Instance { get; private set; }

    [SerializeField] private TextController _goalTextController;
    [SerializeField] private TextController _levelTextController;
    [SerializeField] private TextController _scoresTextController;

    [SerializeField] private GameObject _goalText;

    private const string DEFAULTGOALTEXT = "Goal: {0}";
    private string _defaultLevelText = "Level: {0}";
    private string _defaultGameModeText = "Endless Mode";
    private const string DEFAULTSCORESTEXT = "Scores: {0}";

    private void Awake() 
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }

    private void Start() 
    {
        ChangeGameMode(GameController.Instance.CurrentGameMode);
    }

    public void UpdateGoalText(int score) => UpdateText(_goalTextController, string.Format(DEFAULTGOALTEXT, score));
    public void UpdateScoresText(int points) => UpdateText(_scoresTextController, string.Format(DEFAULTSCORESTEXT, points));

    public void ChangeGameMode(GameController.GameMode gameMode) 
    {
        switch (gameMode) 
        {
            case GameController.GameMode.endless:
                UpdateText(_levelTextController, _defaultGameModeText);
                _goalText.SetActive(false);
                break;

            case GameController.GameMode.levels:
                UpdateText(_levelTextController, string.Format(_defaultLevelText, GameController.Instance.CurrentLevel));
                break;
        }
    }

    private void UpdateText(ITextController textController, string text)
    {
        if (textController != null)
            textController.SetText(text);
    }
}
