using UnityEngine;
using UnityEngine.Events;

public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; }

    [SerializeField] private int[] _levelGoals;
    [SerializeField] private GameObject[] _levelMazes;
    [SerializeField] private Transform _mazeSpawnPoint;
    [SerializeField] private float[] _levelSpawnInterval;
    [SerializeField] private float[] _levelInitialSpeed;
    [SerializeField] private float[] _levelCountAtTime;
    [SerializeField] private float[] _levelSpeedIncreaseRate;
    [Range(0, 30)]
    [SerializeField] private int _difficultyOfEndlessMode = 15;

    [SerializeField] private GameObject _tutorialScreen;

    public enum GameMode { Endless, Levels }
    public GameMode CurrentGameMode { get; private set; }
    public int CurrentLevel { get; private set; } = 0;

    public UnityEvent<bool> OnGameComplete { get; private set; } = new UnityEvent<bool>();

    private int _points = 0;
    private int _caughtBalls = 0;

    private void Awake() 
    {
        InitializeSingleton();
        InitializeGameMode();
        InitializeLevel();
    }

    private void Start() 
    {
        ShowTutorialIfFirstLaunch();
        InitializeGame();
    }

    private void InitializeSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void InitializeGameMode()
    {
        CurrentGameMode = PlayerPrefsController.GetGameMode(GameMode.Endless);
    }

    private void InitializeLevel()
    {
        if (CurrentGameMode == GameMode.Levels)
        {
            CurrentLevel = PlayerPrefsController.GetLevel(0);
        }
    }

    private void ShowTutorialIfFirstLaunch()
    {
        if (PlayerPrefs.GetInt("Show Tutorial", 0) == 0)
        {
            TimeController.Instance.StopTime(0);
            _tutorialScreen.SetActive(true);
            PlayerPrefs.SetInt("Show Tutorial", 1);
        }
    }

    private void InitializeGame()
    {
        SpawnMaze(CurrentGameMode == GameMode.Levels ? CurrentLevel : Random.Range(0, _levelMazes.Length));
        SetupPrefabSpawner(CurrentGameMode == GameMode.Levels ? CurrentLevel : _difficultyOfEndlessMode);
        
        InitializeHUD();
        TimeController.Instance.StartSpeedingUp();
        OnGameComplete.AddListener(CompleteGame);
    }

    private void InitializeHUD()
    {
        HUDController.Instance.UpdateScoresText(_points);
        if (CurrentGameMode == GameMode.Levels)
        {
            HUDController.Instance.UpdateGoalText(_levelGoals[CurrentLevel]);
        }
    }

    private void SetupPrefabSpawner(int index)
    {
        PrefabSpawner.Instance.SpawnInterval = _levelSpawnInterval[index];
        PrefabSpawner.Instance.InitialSpeed = _levelInitialSpeed[index];
        PrefabSpawner.Instance.CountAtTime = _levelCountAtTime[index];
        PrefabSpawner.Instance.SpeedIncreaseRate = _levelSpeedIncreaseRate[index];
    }

    private void SpawnMaze(int index)
    {
        Instantiate(_levelMazes[index], _mazeSpawnPoint.position, Quaternion.identity, _mazeSpawnPoint);
    }

    public void ScorePoints(int points)
    {
        _points += points;
        HUDController.Instance.UpdateScoresText(_points);
    }

    public void ScoreGoal()
    {
        _caughtBalls++;
        if (CurrentGameMode == GameMode.Levels)
        {
            HUDController.Instance.UpdateGoalText(_levelGoals[CurrentLevel] - _caughtBalls);
            if (_caughtBalls >= _levelGoals[CurrentLevel])
            {
                OnGameComplete?.Invoke(true);
            }
        }
    }

    private void CompleteGame(bool isPlayerAWinner)
    {
        PrefabSpawner.Instance.Stop();
        IcePowerController.Instance.IsIcePowerAvailable = false;
        TimeController.Instance.StopSpeedingUp();

        EndGameController.Instance.ShowEndGameScreen(
            isPlayerAWinner, 
            _points, 
            CurrentGameMode == GameMode.Endless, 
            CurrentLevel
        );
    }
}
