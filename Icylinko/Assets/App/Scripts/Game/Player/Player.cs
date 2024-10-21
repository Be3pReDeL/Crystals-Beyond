using UnityEngine;
using UnityEngine.EventSystems;  // Необходимо для работы с UI

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }

    [SerializeField] private int _maxHealth = 100;  // Максимальное здоровье
    private int _currentHealth;  // Текущее здоровье

    [SerializeField] private HealthUIController _healthUIController;  // Ссылка на UI контроллер для здоровья
    [SerializeField] private IcePowerController _icePowerController;  // Ссылка на Ice Power контроллер

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
    }

    private void Update()
    {
        // Проверяем нажатие на экран для активации Ice Power, только если это не UI элемент
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began && !IsPointerOverUI())
        {
            // Проверяем, доступна ли Ice Power, и активируем, если да
            if (_icePowerController.IsIcePowerAvailable())
            {
                _icePowerController.UseIcePower();
            }
        }
    }

    // Метод для проверки, нажимает ли игрок на UI элемент
    private bool IsPointerOverUI()
    {
        return EventSystem.current.IsPointerOverGameObject();  // Проверяем, находится ли указатель мыши или касание на UI элементе
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
