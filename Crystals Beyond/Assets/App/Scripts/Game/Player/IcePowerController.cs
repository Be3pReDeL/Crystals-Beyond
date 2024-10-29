using UnityEngine;

public class IcePowerController : MonoBehaviour
{
    public static IcePowerController Instance { get; private set; }

    [SerializeField] private IcePowerUIController _icePowerUIController;
    [SerializeField] private ScreenController _slowMotionOverlayScreenController;
    [SerializeField] private float _slowMotionDuration = 5f;
    [SerializeField] private Animator[] _playerCircleSegmentsAnimators;

    public bool IsIcePowerAvailable { get; set; } = true;
    public bool IsIcePowerActive { get; private set; } = false;

    public float DefaultRechargeDuration { get; set; } = 7f;
    public float RechargeDuration { get; set; }
    private float _currentRechargeTime = 0f;
    private bool _isRecharging = false;

    private const string _ANIMATORKEY = "Frozen";

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

            TimeController.Instance.StartIcePower();  // Активируем замедление времени через Ice Power

            SetPlayerCircleSegmentsAnimatorsBools(_ANIMATORKEY, true);

            Invoke(nameof(EndSlowMotion), _slowMotionDuration);
        }
    }

    private void EndSlowMotion()
    {
        IsIcePowerActive = false;

        // Завершаем Ice Power
        TimeController.Instance.EndIcePower();

        _slowMotionOverlayScreenController.CloseScreen();
        _isRecharging = true;
        _currentRechargeTime = 0f;

        SetPlayerCircleSegmentsAnimatorsBools(_ANIMATORKEY, false);
    }

    public void ResumeIcePower()
    {
        TimeController.Instance.ResumeIcePower();
        _slowMotionOverlayScreenController.gameObject.SetActive(true);
        SetPlayerCircleSegmentsAnimatorsBools(_ANIMATORKEY, true);
    }

    private void SetPlayerCircleSegmentsAnimatorsBools(string key, bool state) 
    {
        foreach (Animator animator in _playerCircleSegmentsAnimators)
            animator.SetBool(key, state);
    }
}
