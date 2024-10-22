using UnityEngine;
using System.Collections;

public class TimeController : MonoBehaviour
{
    public static TimeController Instance { get; private set; }
    
    private float _currentTimeScale;      // Текущий множитель времени
    private bool _isTimeStopped = false;  // Флаг, указывающий, остановлено ли время
    private float _timeSpeedUpMaximum = 3f;  // Ограничение максимальной скорости
    private float _timeIncreaseRate = 0.001f;   // Скорость увеличения времени
    private bool _isTimeSpeedingUp = false;  // Флаг, указывающий, активно ли ускорение времени

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
    public void StopTime() 
    {
        _isTimeStopped = true;
        _currentTimeScale = Time.timeScale;
        Time.timeScale = 0.1f;
    }

    // Метод для сброса времени
    public void ResetTime() 
    {
        _isTimeStopped = false;
        Time.timeScale = _currentTimeScale;
    }

    // Метод для активации ускорения времени
    public void StartSpeedingUp()
    {
        _isTimeSpeedingUp = true;  // Устанавливаем флаг, что ускорение активно
        StartCoroutine(SpeedUpTime());  // Запускаем корутину для увеличения времени
    }

    // Метод для остановки ускорения времени
    public void StopSpeedingUp()
    {
        _isTimeSpeedingUp = false;  // Останавливаем ускорение
    }

    // Корутин для постепенного увеличения времени
    private IEnumerator SpeedUpTime()
    {
        while (_isTimeSpeedingUp)
        {
            if (Time.timeScale < _timeSpeedUpMaximum)  // Ограничиваем максимальную скорость
                Time.timeScale += _timeIncreaseRate;  // Увеличиваем timeScale
            
            yield return new WaitForSeconds(0.05f);  // Ждем перед следующим увеличением
        }
    }
}
