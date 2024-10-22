using UnityEngine;

public class IcePowerController : MonoBehaviour
{
    public static IcePowerController Instance { get; private set; }

    [SerializeField] private IcePowerUIController _icePowerUIController;  // Ссылка на новый контроллер для управления UI Slider
    [SerializeField] private ScreenController _slowMotionOverlayScreenController;
    [SerializeField] private float _rechargeDuration = 7f;  // Время для полного восстановления
    [SerializeField] private float _slowMotionDuration = 5f;  // Время замедления

    public bool IsIcePowerAvailable { get; set; } = true;  // Доступно ли замедление
    private float _currentRechargeTime = 0f;  // Текущее время восстановления
    private bool _isRecharging = false;

    private void Awake() 
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }

    private void Start()
    {
        // Инициализируем Ice Power UI с максимальным значением 1 и текущим значением 1 (полная шкала)
        _icePowerUIController.InitializeIcePowerUI(1f, 1f);
        _slowMotionOverlayScreenController.gameObject.SetActive(false);  // Отключаем overlay в начале
    }

    private void Update()
    {
        if (_isRecharging)
        {
            // Восстановление Ice Power
            _currentRechargeTime += Time.deltaTime;
            float newValue = Mathf.Clamp01(_currentRechargeTime / _rechargeDuration);

            _icePowerUIController.UpdateIcePowerUI(newValue);  // Обновляем значение на UI

            if (newValue >= 1f)
            {
                _isRecharging = false;
                IsIcePowerAvailable = true;
            }
        }
    }

    public void UseIcePower()
    {
        if (IsIcePowerAvailable)
        {
            IsIcePowerAvailable = false;
            _isRecharging = false;
            _icePowerUIController.UpdateIcePowerUI(0f);  // Полностью расходуем шкалу
            _slowMotionOverlayScreenController.gameObject.SetActive(true);  // Включаем эффект замедления
            Time.timeScale = 0.5f;  // Замедляем время

            Invoke(nameof(EndSlowMotion), _slowMotionDuration);  // Через 5 секунд отключаем замедление
        }
    }

    private void EndSlowMotion()
    {
        Time.timeScale = 1f;  // Возвращаем время в нормальное состояние
        _slowMotionOverlayScreenController.CloseScreen();  // Отключаем overlay
        _isRecharging = true;  // Начинаем восстановление Ice Power
        _currentRechargeTime = 0f;
    }
}
