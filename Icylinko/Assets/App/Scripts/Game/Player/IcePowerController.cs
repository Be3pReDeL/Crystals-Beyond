using UnityEngine;

public class IcePowerController : MonoBehaviour
{
    public static IcePowerController Instance { get; private set; }

    [SerializeField] private IcePowerUIController _icePowerUIController;
    [SerializeField] private ScreenController _slowMotionOverlayScreenController;
    [SerializeField] private float _slowMotionDuration = 5f;

    public bool IsIcePowerAvailable { get; set; } = true;
    public float DefaultRechargeDuration { get; set; } = 7f;
    public float RechargeDuration { get; set; }
    private float _currentRechargeTime = 0f;
    private bool _isRecharging = false;
    private bool _isIcePowerActive = false; // Новый флаг для отслеживания активности Ice Power перед паузой
    private float _remainingSlowMotionTime = 0f;  // Время, которое осталось до окончания действия Ice Power

    private void Awake() 
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);

        RechargeDuration = DefaultRechargeDuration;
    }

    private void Start()
    {
        _icePowerUIController.InitializeIcePowerUI(1f, 1f);
        _slowMotionOverlayScreenController.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (_isRecharging)
        {
            _currentRechargeTime += Time.deltaTime;
            float newValue = Mathf.Clamp01(_currentRechargeTime / RechargeDuration);

            _icePowerUIController.UpdateIcePowerUI(newValue);

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
            _isIcePowerActive = true;  // Устанавливаем флаг активности способности Ice Power
            _icePowerUIController.UpdateIcePowerUI(0f);
            _slowMotionOverlayScreenController.gameObject.SetActive(true);
            
            _remainingSlowMotionTime = _slowMotionDuration;
            TimeController.Instance.StopTime(0.5f);

            Invoke(nameof(EndSlowMotion), _slowMotionDuration);
        }
    }

    private void EndSlowMotion()
    {
        _isIcePowerActive = false;  // Сбрасываем флаг активности Ice Power
        TimeController.Instance.ResetTime();
        _slowMotionOverlayScreenController.CloseScreen();
        _isRecharging = true;
        _currentRechargeTime = 0f;
    }

    // Новый метод, который вызывается при нажатии паузы
    public void PauseIcePower()
    {
        if (_isIcePowerActive)
        {
            // Отменяем Invoke для завершения замедления времени, чтобы оно не истекло на паузе
            CancelInvoke(nameof(EndSlowMotion));
        }
    }

    // Новый метод, который вызывается при возобновлении игры
    public void ResumeIcePower()
    {
        if (_isIcePowerActive)
        {
            // Продолжаем действие способности Ice Power
            Invoke(nameof(EndSlowMotion), _remainingSlowMotionTime);
        }
    }
}
