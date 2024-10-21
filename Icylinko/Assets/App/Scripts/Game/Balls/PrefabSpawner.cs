using UnityEngine;
using System.Collections.Generic;

public class PrefabSpawner : MonoBehaviour
{
    public static PrefabSpawner Instance { get; private set; }

    [SerializeField] private GameObject[] _prefabs;  // Массив префабов для спавна
    [SerializeField] private Transform _spawnPoint;  // Точка спавна
    public float SpawnInterval { get; set; } = 1f;  // Интервал между спавнами
    public int TotalPrefabs { get; set; } = 1;  // Общее количество префабов
    public float InitialSpeed { get; set; } = 2f;  // Начальная скорость объектов
    public float SpeedIncreaseRate { get; set; } = 0.5f;  // Скорость прироста

    private int _spawnedCount = 0;  // Счетчик созданных префабов
    private float _spawnTimer = 0f;  // Таймер для спавна
    private bool _isSpawningActive = true;

    private List<GameObject> _balls = new List<GameObject>();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }

    private void Update()
    {
        if(_isSpawningActive)
        {
            if (_spawnedCount >= TotalPrefabs && GameController.Instance.CurrentGameMode == GameController.GameMode.levels)
            {
                if (_balls.Count == 0 && GameController.Instance.IsGameContinues) 
                {
                    GameController.Instance.OnGameComplete.Invoke(false);

                    enabled = false;
                }

                return;
            }

            _spawnTimer += Time.deltaTime;
            if (_spawnTimer >= SpawnInterval)
            {
                SpawnPrefab();  // Спавн объекта
                _spawnTimer = 0f;
            }
        }
    }

    private void SpawnPrefab()
    {
        // Выбираем случайный префаб из массива
        GameObject randomPrefab = _prefabs[Random.Range(0, _prefabs.Length)];

        GameObject newPrefab = Instantiate(randomPrefab, _spawnPoint.position, Quaternion.identity, _spawnPoint);

        _balls.Add(newPrefab);

        // Получаем компонент Rigidbody2D
        Rigidbody2D rb = newPrefab.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            // Отключаем влияние гравитации
            rb.gravityScale = 0;

            // Генерируем случайный угол в диапазоне 0-360 градусов
            float randomAngle = Random.Range(0f, 360f);
            Vector2 initialDirection = new Vector2(Mathf.Cos(randomAngle * Mathf.Deg2Rad), Mathf.Sin(randomAngle * Mathf.Deg2Rad));

            // Присваиваем начальное направление и скорость
            rb.velocity = initialDirection * InitialSpeed;

            // Добавляем скрипт, который будет управлять ускорением объекта
            BallMovement ballMovement = newPrefab.GetComponent<BallMovement>();
            ballMovement.Setup(rb, InitialSpeed, SpeedIncreaseRate);
        }

        _spawnedCount++;
    }

    public void DeleteBallFromList(GameObject ball) => _balls.Remove(ball);

    public void Stop() 
    {
        _isSpawningActive = false;
        
        enabled = false;
    }
}
