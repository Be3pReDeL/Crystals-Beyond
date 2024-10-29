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

    public delegate void PlayerDeathHandler();
    public event PlayerDeathHandler OnPlayerDeath;  // Событие для обработки смерти

    private void Awake() 
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        if (_healthUIController == null)
        {
            Debug.LogError("Health UI Controller не назначен в инспекторе!");
            return;
        }

        if (_icePowerController == null)
        {
            Debug.LogError("Ice Power Controller не назначен в инспекторе!");
            return;
        }

        if (_icePowerButton != null)
        {
            _icePowerButton.onClick.AddListener(ActivateIcePower);
        }
        else
        {
            Debug.LogError("Ice Power Button не назначен в инспекторе!");
        }

        _currentHealth = _maxHealth;
        _healthUIController.InitializeHealthUI(_maxHealth, _currentHealth);
    }

    private void ActivateIcePower()
    {
        if (_icePowerController.IsIcePowerAvailable)
        {
            _icePowerController.UseIcePower();
        }
    }

    public void TakeDamage(int damage)
    {
        _currentHealth -= damage;
        _currentHealth = Mathf.Clamp(_currentHealth, 0, _maxHealth);

        _healthUIController.UpdateHealthUI(_currentHealth);

        if (_currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(int amount)
    {
        _currentHealth += amount;
        _currentHealth = Mathf.Clamp(_currentHealth, 0, _maxHealth);

        _healthUIController.UpdateHealthUI(_currentHealth);
    }

    private void Die()
    {
        if (Instance == this)
        {
            OnPlayerDeath?.Invoke();  // Вызываем событие смерти
            GameController.Instance?.OnGameComplete?.Invoke(false);
            Destroy(gameObject);  // Удаляем объект игрока, если нужно
        }
    }
}
