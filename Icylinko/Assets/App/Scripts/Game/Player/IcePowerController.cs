using UnityEngine;
using UnityEngine.UI;

public class IcePowerController : MonoBehaviour
{
    [SerializeField] private IcePowerUIController _icePowerUIController;  // Ссылка на новый контроллер для управления UI Slider
    [SerializeField] private Image _slowMotionOverlay;  // UI Image для замедления времени
    [SerializeField] private float _rechargeDuration = 7f;  // Время для полного восстановления
    [SerializeField] private float _slowMotionDuration = 5f;  // Время замедления

    private bool _isIcePowerAvailable = true;  // Доступно ли замедление
    private float _currentRechargeTime = 0f;  // Текущее время восстановления
    private bool _isRecharging = false;

    private void Start()
    {
        // Инициализируем Ice Power UI с максимальным значением 1 и текущим значением 1 (полная шкала)
        _icePowerUIController.InitializeIcePowerUI(1f, 1f);
        _slowMotionOverlay.gameObject.SetActive(false);  // Отключаем overlay в начале
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
                _isIcePowerAvailable = true;
            }
        }
    }

    public void UseIcePower()
    {
        if (_isIcePowerAvailable)
        {
            _isIcePowerAvailable = false;
            _isRecharging = false;
            _icePowerUIController.UpdateIcePowerUI(0f);  // Полностью расходуем шкалу
            _slowMotionOverlay.gameObject.SetActive(true);  // Включаем эффект замедления
            Time.timeScale = 0.5f;  // Замедляем время

            Invoke(nameof(EndSlowMotion), _slowMotionDuration);  // Через 5 секунд отключаем замедление
        }
    }

    private void EndSlowMotion()
    {
        Time.timeScale = 1f;  // Возвращаем время в нормальное состояние
        _slowMotionOverlay.gameObject.SetActive(false);  // Отключаем overlay
        _isRecharging = true;  // Начинаем восстановление Ice Power
        _currentRechargeTime = 0f;
    }

    public bool IsIcePowerAvailable()
    {
        return _isIcePowerAvailable;
    }
}
