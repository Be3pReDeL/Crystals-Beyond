using UnityEngine;

public class TimeController : MonoBehaviour
{
    public static TimeController Instance { get; private set; }
    private float _currentTimeScale;
    private bool _isTimeStopped = false;

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

    public void StopTime() 
    {
        _isTimeStopped = true;

        _currentTimeScale = Time.timeScale;

        Time.timeScale = 0.05f;
    }

    public void ResetTime() 
    {
        _isTimeStopped = false;

        Time.timeScale = _currentTimeScale;
    }
}
