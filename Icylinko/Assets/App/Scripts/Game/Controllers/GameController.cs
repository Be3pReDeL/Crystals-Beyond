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
    [SerializeField] private int _dificultyOfEndlessMode = 15;

    public enum GameMode { endless, levels }
    public GameMode CurrentGameMode { get; private set; }
    public int CurrentLevel { get; private set; } = 0;

    public UnityEvent<bool> OnGameComplete;

    private int _points = 0; // Текущий счет
    private int _caughtBalls = 0; // Количество пойманных шариков

    private void Awake() 
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);

        if (OnGameComplete == null)
            OnGameComplete = new UnityEvent<bool>();

        PlayerPrefs.SetInt("Game Mode", (int) GameMode.levels);
        PlayerPrefs.SetInt("Level", 15);

        CurrentGameMode = (GameMode) PlayerPrefs.GetInt("Game Mode", (int) GameMode.endless);

        if (CurrentGameMode == GameMode.levels)
            CurrentLevel = PlayerPrefs.GetInt("Level", 0);
    }

    private void Start() 
    {
        SpawnMaze(CurrentGameMode == GameMode.levels ? CurrentLevel : Random.Range(0, _levelMazes.Length));
        SetupPrefabSpawner(CurrentGameMode == GameMode.levels ? CurrentLevel : _dificultyOfEndlessMode);

        HUDController.Instance.UpdateScoresText(0);
        if (CurrentGameMode == GameMode.levels)
            HUDController.Instance.UpdateGoalText(_levelGoals[CurrentLevel]);

        OnGameComplete.AddListener(CompleteGame);
    }

    private void SetupPrefabSpawner(int type)
    {
        PrefabSpawner.Instance.SpawnInterval = _levelSpawnInterval[type];
        PrefabSpawner.Instance.InitialSpeed = _levelInitialSpeed[type];
        PrefabSpawner.Instance.CountAtTime = _levelCountAtTime[type];
        PrefabSpawner.Instance.SpeedIncreaseRate = _levelSpeedIncreaseRate[type];
    }

    private void SpawnMaze(int type) => Instantiate(_levelMazes[type], _mazeSpawnPoint.position, Quaternion.identity, _mazeSpawnPoint);

    public void ScorePoints(int points)
    {
        _points += points; // Увеличиваем счет за выигрышный сегмент

        HUDController.Instance.UpdateScoresText(_points); // Обновляем число заработанных очков
    }

    public void ScoreGoal()
    {
        _caughtBalls++; // Увеличиваем счет за выигрышный сегмент

        HUDController.Instance.UpdateGoalText(_levelGoals[CurrentLevel] - _caughtBalls); // Обновляем число заработанных очков

        if(_caughtBalls == _levelGoals[CurrentLevel])
            OnGameComplete?.Invoke(true);
    }

    private void CompleteGame(bool isPlayerAWinner) 
    {
        PrefabSpawner.Instance.Stop();
        
        Debug.Log("SO PLAYER IS A " + isPlayerAWinner.ToString());
    }
}