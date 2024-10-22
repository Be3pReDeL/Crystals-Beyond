using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }

    [SerializeField] private int _maxHealth = 100;  // Максимальное здоровье
    private int _currentHealth;  // Текущее здоровье

    [SerializeField] private HealthUIController _healthUIController;  // Ссылка на UI контроллер для здоровья
    [SerializeField] private IcePowerController _icePowerController;  // Ссылка на Ice Power контроллер
    [SerializeField] private Button _icePowerButton;  // Ссылка на кнопку Ice Power

    private void Awake() 
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }

    private void Start()
    {
        // Инициализируем здоровье игрока
        _currentHealth = _maxHealth;
        _healthUIController.InitializeHealthUI(_maxHealth, _currentHealth);

        // Добавляем слушателя нажатия на кнопку
        _icePowerButton.onClick.AddListener(ActivateIcePower);
    }

    // Метод для активации Ice Power
    private void ActivateIcePower()
    {
        if (_icePowerController.IsIcePowerAvailable)
        {
            Debug.Log("Ice Power активирован!");
            _icePowerController.UseIcePower();
        }
        else
        {
            Debug.Log("Ice Power недоступен.");
        }
    }

    // Метод для получения урона
    public void TakeDamage(int damage)
    {
        _currentHealth -= damage;  // Уменьшаем здоровье игрока
        _currentHealth = Mathf.Clamp(_currentHealth, 0, _maxHealth);  // Ограничиваем здоровье в пределах 0 и максимума

        // Обновляем здоровье на UI
        _healthUIController.UpdateHealthUI(_currentHealth);

        // Проверка, достиг ли игрок нуля здоровья
        if (_currentHealth <= 0)
        {
            Die();  // Вызываем метод смерти игрока
        }
    }

    // Метод для обработки смерти игрока
    private void Die()
    {
        // Обрабатываем смерть (например, завершение игры)
        GameController.Instance.OnGameComplete?.Invoke(false);  // Сообщаем GameController о завершении игры
    }
}
