using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public static PlayerHealth Instance { get; private set; }

    [SerializeField] private int _maxHealth = 100;  // Максимальное здоровье
    private int _currentHealth;  // Текущее здоровье

    [SerializeField] private HealthUIController _healthUIController;  // Ссылка на UI контроллер для здоровья

    private void Awake() 
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }

    private void Start()
    {
        // Устанавливаем начальное здоровье и инициализируем UI
        _currentHealth = _maxHealth;
        _healthUIController.InitializeHealthUI(_maxHealth, _currentHealth);
    }

    // Метод для нанесения урона
    public void TakeDamage(int damage)
    {
        _currentHealth -= damage;  // Уменьшаем здоровье игрока
        _currentHealth = Mathf.Clamp(_currentHealth, 0, _maxHealth);  // Ограничиваем значение здоровья

        // Обновляем здоровье на UI
        _healthUIController.UpdateHealthUI(_currentHealth);

        // Проверка, достигло ли здоровье нуля
        if (_currentHealth <= 0)
        {
            Die();  // Обрабатываем смерть игрока
        }
    }

    // Метод, который срабатывает при смерти игрока
    private void Die()
    {
        // Можно добавить логику для обработки смерти (например, проигрыш)
        GameController.Instance.OnGameComplete?.Invoke(false);  // Сообщаем GameController о смерти игрока
    }
}
