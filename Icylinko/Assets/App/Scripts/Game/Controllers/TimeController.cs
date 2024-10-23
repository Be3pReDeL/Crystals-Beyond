using UnityEngine;
using System.Collections;

public class TimeController : MonoBehaviour
{
    public static TimeController Instance { get; private set; }

    private float _currentTimeScale = -1; // Изначально значение не инициализировано
    private bool _isTimeStopped = false;  
    private float _timeSpeedUpMaximum = 3f;  
    private float _timeIncreaseRate = 0.001f;  
    private bool _isTimeSpeedingUp = false;

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

    // Метод для остановки времени
    public void StopTime(float newTimeScale) 
    {
        _isTimeStopped = true;

        if (_currentTimeScale < 0)  // Сохраняем значение текущего TimeScale при первом вызове StopTime
            _currentTimeScale = Time.timeScale;

        Time.timeScale = Mathf.Max(0.0f, newTimeScale);  // Защищаем Time.timeScale от значений меньше 0

        StopSpeedingUp();

        // Останавливаем Ice Power, если оно активно
        IcePowerController.Instance?.PauseIcePower();
    }

    // Метод для сброса времени
    public void ResetTime() 
    {
        if (_isTimeStopped)
        {
            _isTimeStopped = false;

            // Проверяем, что _currentTimeScale корректен и не отрицателен
            if (_currentTimeScale >= 0)
            {
                Time.timeScale = Mathf.Max(0.0f, _currentTimeScale);  // Устанавливаем Time.timeScale, не меньше 0
            }
            else
            {
                // Если _currentTimeScale был инициализирован неправильно, устанавливаем 1.0f по умолчанию
                Time.timeScale = 1.0f;
            }

            _currentTimeScale = -1;  // Сбрасываем текущий timeScale для следующих вызовов StopTime
            StartSpeedingUp();
            
            // Возобновляем Ice Power, если оно было активно
            IcePowerController.Instance?.ResumeIcePower();
        }
    }

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
            if (Time.timeScale < _timeSpeedUpMaximum)  // Ограничиваем максимальную скорость
                Time.timeScale += _timeIncreaseRate;
            
            yield return new WaitForSeconds(0.05f);  // Ждем перед следующим увеличением
        }
    }
}
