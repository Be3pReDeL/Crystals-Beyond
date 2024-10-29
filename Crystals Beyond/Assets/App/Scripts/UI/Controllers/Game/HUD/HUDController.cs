using UnityEngine;

public class HUDController : MonoBehaviour
{
    public static HUDController Instance { get; private set; }

    [SerializeField] private TextController _goalTextController;
    [SerializeField] private TextController _levelTextController;
    [SerializeField] private TextController _scoresTextController;

    [SerializeField] private GameObject _goalText;

    private const string EndlessModeText = "Endless Mode";

    private void Awake() 
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start() 
    {
        ChangeGameMode(GameController.Instance.CurrentGameMode);
    }

    public void SetGoal(int goal) => UpdateText(_goalTextController, $"Goal: {Mathf.Max(0, goal)}");
    public void SetScore(int score) => UpdateText(_scoresTextController, $"Scores: {score}");
    public void SetLevel(int level) => UpdateText(_levelTextController, $"Level: {level}");

    public void ChangeGameMode(GameController.GameMode gameMode) 
    {
        switch (gameMode) 
        {
            case GameController.GameMode.Endless:
                UpdateText(_levelTextController, EndlessModeText);
                _goalText.SetActive(false);
                break;

            case GameController.GameMode.Levels:
                SetLevel(GameController.Instance.CurrentLevel + 1);
                _goalText.SetActive(true);
                break;
        }
    }

    private void UpdateText(TextController textController, string text)
    {
        textController?.SetText(text);
    }
}
