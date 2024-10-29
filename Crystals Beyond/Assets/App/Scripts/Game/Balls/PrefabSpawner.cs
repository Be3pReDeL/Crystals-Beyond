using UnityEngine;

public class PrefabSpawner : MonoBehaviour
{
    public static PrefabSpawner Instance { get; private set; }

    [SerializeField] private GameObject[] _prefabs;  // Массив префабов для спавна
    [SerializeField] private Transform _spawnPoint;  // Точка спавна
    [SerializeField] private AudioSource _audioSource;

    public float SpawnInterval { get; set; } = 1f;  // Интервал между спавнами
    public float InitialSpeed { get; set; } = 2f;  // Начальная скорость объектов
    public float CountAtTime { get; set; } = 1f;  // Количество за раз
    public float SpeedIncreaseRate { get; set; } = 0.5f;  // Скорость прироста

    private float _spawnTimer = 0f;  // Таймер для спавна
    private bool _isSpawningActive = true;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        _audioSource = _audioSource ?? GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (!_isSpawningActive) return;

        _spawnTimer += Time.deltaTime;
        if (_spawnTimer >= SpawnInterval)
        {
            SpawnMultiplePrefabs();
            _spawnTimer = 0f;
        }
    }

    private void SpawnMultiplePrefabs()
    {
        for (int i = 0; i < CountAtTime; i++)
            SpawnPrefab();
    }

    private void SpawnPrefab()
    {
        // Выбор случайного префаба и создание экземпляра
        if (_prefabs.Length == 0) return;

        GameObject randomPrefab = _prefabs[Random.Range(0, _prefabs.Length)];
        GameObject newPrefab = Instantiate(randomPrefab, _spawnPoint.position, Quaternion.identity, _spawnPoint);

        // Проигрывание звука спавна
        _audioSource?.Play();

        // Установка начальной скорости и направления для Rigidbody2D
        var rb = newPrefab.GetComponent<Rigidbody2D>();
        if (rb != null)
            InitializeRigidbody(rb);

        // Вибрация при спавне
        VibrationController.Instance?.Vibrate(VibrationController.VibrationType.light);
    }

    private void InitializeRigidbody(Rigidbody2D rb)
    {
        rb.gravityScale = 0;
        float randomAngle = Random.Range(0f, 360f);
        Vector2 initialDirection = new Vector2(Mathf.Cos(randomAngle * Mathf.Deg2Rad), Mathf.Sin(randomAngle * Mathf.Deg2Rad));
        rb.velocity = initialDirection * InitialSpeed;

        var ballMovement = rb.GetComponent<BallMovement>();
        ballMovement?.Setup(InitialSpeed, SpeedIncreaseRate);
    }

    public void Stop()
    {
        _isSpawningActive = false;
        enabled = false;
    }
}
