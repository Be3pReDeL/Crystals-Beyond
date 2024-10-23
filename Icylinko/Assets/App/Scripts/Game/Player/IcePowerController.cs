using UnityEngine;

public class IcePowerController : MonoBehaviour
{
    public static IcePowerController Instance { get; private set; }

    [SerializeField] private IcePowerUIController _icePowerUIController;
    [SerializeField] private ScreenController _slowMotionOverlayScreenController;
    [SerializeField] private float _slowMotionDuration = 5f;

    public bool IsIcePowerAvailable { get; set; } = true;
    public bool IsIcePowerActive { get; private set; } = false;

    public float DefaultRechargeDuration { get; set; } = 7f;
    public float RechargeDuration { get; set; }
    private float _currentRechargeTime = 0f;
    private bool _isRecharging = false;

    private float _preIcePowerTimeScale;  // Значение времени до активации Ice Power
    private float _duringIcePowerTimeScale;  // Значение времени, когда Ice Power была активирована

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
        if (IsIcePowerAvailable && !IsIcePowerActive)
        {
            IsIcePowerAvailable = false;
            IsIcePowerActive = true;
            _isRecharging = false;
            _icePowerUIController.UpdateIcePowerUI(0f);
            _slowMotionOverlayScreenController.gameObject.SetActive(true);

            _preIcePowerTimeScale = Time.timeScale;  // Сохраняем значение Time.timeScale до активации Ice Power
            _duringIcePowerTimeScale = 0.5f;  // Время во время Ice Power
            TimeController.Instance.StopTime(_duringIcePowerTimeScale);  // Устанавливаем замедление времени

            Invoke(nameof(EndSlowMotion), _slowMotionDuration);
        }
    }

    private void EndSlowMotion()
    {
        IsIcePowerActive = false;

        // Проверяем, была ли пауза активирована
        if (TimeController.Instance.IsTimeStopped)
        {
            _slowMotionOverlayScreenController.CloseScreen();
        }
        else
        {
            // Восстанавливаем значение времени до Ice Power
            TimeController.Instance.SetTimeScale(_preIcePowerTimeScale);
            _slowMotionOverlayScreenController.CloseScreen();
        }

        _isRecharging = true;
        _currentRechargeTime = 0f;
    }

    // Возвращаем Ice Power эффект после выхода из паузы
    public void ResumeIcePower()
    {
        TimeController.Instance.SetTimeScale(_duringIcePowerTimeScale);
        _slowMotionOverlayScreenController.gameObject.SetActive(true);
    }
}
