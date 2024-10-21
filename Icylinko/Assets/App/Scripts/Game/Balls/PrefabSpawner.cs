using UnityEngine;
using UnityEngine.Events;

public class PrefabSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] _prefabs;  // Массив префабов для спавна
    [SerializeField] private Transform _spawnPoint;  // Точка спавна
    [SerializeField] private float _spawnInterval = 1f;  // Интервал между спавнами
    [SerializeField] private int _totalPrefabs = 10;  // Общее количество префабов
    [SerializeField] private float _initialSpeed = 2f;  // Начальная скорость объектов
    [SerializeField] private float _speedIncreaseRate = 0.5f;  // Скорость прироста
    
    public UnityEvent OnSpawningComplete;  // Событие на конец спавна

    private int _spawnedCount = 0;  // Счетчик созданных префабов
    private float _spawnTimer = 0f;  // Таймер для спавна

    private void Awake()
    {
        if (OnSpawningComplete == null)
            OnSpawningComplete = new UnityEvent();
    }

    private void Update()
    {
        if (_spawnedCount >= _totalPrefabs)
        {
            OnSpawningComplete.Invoke();  // Вызываем событие завершения спавна
            enabled = false;
            return;
        }

        _spawnTimer += Time.deltaTime;
        if (_spawnTimer >= _spawnInterval)
        {
            SpawnPrefab();  // Спавн объекта
            _spawnTimer = 0f;
        }
    }

    private void SpawnPrefab()
    {
        // Выбираем случайный префаб из массива
        GameObject randomPrefab = _prefabs[Random.Range(0, _prefabs.Length)];

        GameObject newPrefab = Instantiate(randomPrefab, _spawnPoint.position, Quaternion.identity, _spawnPoint);

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
            rb.velocity = initialDirection * _initialSpeed;

            // Добавляем скрипт, который будет управлять ускорением объекта
            MovingObject movingObject = newPrefab.GetComponent<MovingObject>();
            movingObject.Setup(rb, _initialSpeed, _speedIncreaseRate);
        }

        _spawnedCount++;
    }
}
