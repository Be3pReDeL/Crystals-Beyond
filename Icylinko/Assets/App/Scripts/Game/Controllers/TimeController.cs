using UnityEngine;
using System.Collections;

public class TimeController : MonoBehaviour
{
    public static TimeController Instance { get; private set; }

    private float _currentTimeScale = -1;  // Текущий множитель времени до паузы
    private bool _isTimeStopped = false;
    private float _timeSpeedUpMaximum = 3f;
    private float _timeIncreaseRate = 0.001f;
    private bool _isTimeSpeedingUp = false;

    public bool IsTimeStopped => _isTimeStopped;

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

    // Метод для остановки времени (например, пауза)
    public void StopTime(float newTimeScale) 
    {
        _isTimeStopped = true;

        if (_currentTimeScale < 0)
            _currentTimeScale = Time.timeScale;

        Time.timeScale = newTimeScale;

        StopSpeedingUp();
    }

    // Метод для сброса времени после паузы
    public void ResetTime() 
    {
        _isTimeStopped = false;

        // Проверяем, активна ли способность Ice Power
        if (IcePowerController.Instance != null && IcePowerController.Instance.IsIcePowerActive)
        {
            IcePowerController.Instance.ResumeIcePower();  // Возвращаем замедление Ice Power
        }
        else
        {
            // Восстанавливаем время, которое было до паузы
            Time.timeScale = _currentTimeScale > 0 ? _currentTimeScale : 1f;
        }

        _currentTimeScale = -1;

        StartSpeedingUp();
    }

    // Метод для установки времени вручную
    public void SetTimeScale(float timeScale)
    {
        Time.timeScale = timeScale;
    }

    // Метод для активации ускорения времени
    public void StartSpeedingUp()
    {
        _isTimeSpeedingUp = true;
        StartCoroutine(SpeedUpTime());
    }

    // Метод для остановки ускорения времени
    public void StopSpeedingUp()
    {
        _isTimeSpeedingUp = false;
    }

    // Корутин для постепенного увеличения времени
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
