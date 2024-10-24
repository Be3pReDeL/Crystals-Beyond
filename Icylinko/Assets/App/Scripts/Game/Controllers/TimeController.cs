using UnityEngine;
using System.Collections;

public class TimeController : MonoBehaviour
{
    public static TimeController Instance { get; private set; }

    private float _prePauseTimeScale = 1f;  // Значение timeScale перед паузой
    private float _preIcePowerTimeScale = 1f;  // Значение timeScale перед Ice Power
    private bool _isTimeStopped = false;
    private bool _isIcePowerActive = false;

    private float _timeSpeedUpMaximum = 3f;
    private float _timeIncreaseRate = 0.001f;
    private bool _isTimeSpeedingUp = false;

    public bool IsTimeStopped => _isTimeStopped;
    public bool IsIcePowerActive => _isIcePowerActive;

    private void Awake() 
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }

    private void Start() 
    {
        Time.timeScale = 1.0f;
    }

    // Останавливаем время для паузы
    public void StopTime(float newTimeScale) 
    {
        if (!_isTimeStopped)
        {
            _prePauseTimeScale = Time.timeScale;
            _isTimeStopped = true;
            Time.timeScale = newTimeScale;
        }
    }

    // Сбрасываем время после паузы
    public void ResetTime() 
    {
        _isTimeStopped = false;

        if (_isIcePowerActive)
        {
            // Если Ice Power была активна на момент паузы, возобновляем ее
            Time.timeScale = 0.5f;
        }
        else
        {
            // Восстанавливаем значение перед паузой
            Time.timeScale = _prePauseTimeScale;
            StartSpeedingUp();
        }
    }

    // Останавливаем время для Ice Power
    public void StartIcePower()
    {
        if (!_isIcePowerActive)
        {
            _preIcePowerTimeScale = Time.timeScale;
            _isIcePowerActive = true;
            StopSpeedingUp();
            Time.timeScale = 0.5f;  // Замедляем время до 0.5
        }
    }

    // Завершаем Ice Power и восстанавливаем timeScale
    public void EndIcePower()
    {
        if (_isIcePowerActive)
        {
            _isIcePowerActive = false;
            Time.timeScale = _preIcePowerTimeScale;  // Восстанавливаем значение до активации Ice Power
            StartSpeedingUp();  // Включаем ускорение времени
        }
    }

    // Возобновляем Ice Power после паузы
    public void ResumeIcePower()
    {
        Time.timeScale = 0.5f;
    }

    // Ускорение времени
    public void StartSpeedingUp()
    {
        _isTimeSpeedingUp = true;
        StartCoroutine(SpeedUpTime());
    }

    public void StopSpeedingUp()
    {
        _isTimeSpeedingUp = false;
    }

    private IEnumerator SpeedUpTime()
    {
        while (_isTimeSpeedingUp)
        {
            if (Time.timeScale < _timeSpeedUpMaximum)
                Time.timeScale += _timeIncreaseRate;

            yield return new WaitForSeconds(0.05f);
        }
    }
}
