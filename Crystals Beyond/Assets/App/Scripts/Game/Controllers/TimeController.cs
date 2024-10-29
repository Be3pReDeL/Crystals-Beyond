using UnityEngine;
using System.Collections;

public class TimeController : MonoBehaviour
{
    public static TimeController Instance { get; private set; }

    private float _prePauseTimeScale = 1f;
    private float _preIcePowerTimeScale = 1f;
    private bool _isTimeStopped = false;
    private bool _isIcePowerActive = false;

    private const float _timeSpeedUpMaximum = 3f;
    private const float _timeIncreaseRate = 0.001f;
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

    public void StopTime(float newTimeScale) 
    {
        if (!_isTimeStopped)
        {
            _prePauseTimeScale = Time.timeScale;
            _isTimeStopped = true;
            Time.timeScale = newTimeScale;
        }
    }

    public void ResetTime() 
    {
        _isTimeStopped = false;
        Time.timeScale = _isIcePowerActive ? 0.5f : _prePauseTimeScale;

        if (!_isIcePowerActive)
            StartSpeedingUp();
    }

    public void StartIcePower()
    {
        if (!_isIcePowerActive)
        {
            _preIcePowerTimeScale = Time.timeScale;
            _isIcePowerActive = true;
            StopSpeedingUp();
            Time.timeScale = 0.5f;
        }
    }

    public void EndIcePower()
    {
        if (_isIcePowerActive)
        {
            _isIcePowerActive = false;
            Time.timeScale = _preIcePowerTimeScale;
            StartSpeedingUp();
        }
    }

    public void ResumeIcePower()
    {
        if (_isIcePowerActive)
            Time.timeScale = 0.5f;
    }

    public void StartSpeedingUp()
    {
        if (_isTimeSpeedingUp) return;  // Предотвращаем повторный запуск
        _isTimeSpeedingUp = true;
        StartCoroutine(SpeedUpTime());
    }

    public void StopSpeedingUp()
    {
        _isTimeSpeedingUp = false;
    }

    private IEnumerator SpeedUpTime()
    {
        while (_isTimeSpeedingUp && Time.timeScale < _timeSpeedUpMaximum)
        {
            Time.timeScale += _timeIncreaseRate;
            yield return new WaitForSeconds(0.05f);
        }
    }
}
