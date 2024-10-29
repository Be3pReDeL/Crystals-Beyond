using UnityEngine;

public class RotateObjectWithGyro : MonoBehaviour
{
    private enum RotateAxis { x, y, z }
    [SerializeField] private RotateAxis _axis;
    [SerializeField] private float _multiplier = 1f;
    [SerializeField] private float _smoothSpeed = 5f; // Скорость сглаживания

    private bool _gyroEnabled;
    private Gyroscope _gyro;
    private Vector3 _initialRotation;

    private void Start()
    {
        _gyroEnabled = EnableGyro();
        _initialRotation = transform.localEulerAngles;
    }

    private void Update()
    {
        if (_gyroEnabled)
        {
            Quaternion deviceRotation = GyroToUnity(_gyro.attitude);
            Vector3 eulerAngles = deviceRotation.eulerAngles;
            
            // Вычисляем значение вращения по выбранной оси с учетом _multiplier и _smoothSpeed
            float targetRotation = eulerAngles[(int)_axis] * _multiplier;
            float smoothedRotation = Mathf.LerpAngle(
                transform.localEulerAngles[(int)_axis], 
                _initialRotation[(int)_axis] + targetRotation, 
                Time.deltaTime * _smoothSpeed
            );

            // Устанавливаем плавное вращение только по выбранной оси
            Vector3 newRotation = transform.localEulerAngles;
            newRotation[(int)_axis] = smoothedRotation;
            transform.localEulerAngles = newRotation;
        }
    }

    private bool EnableGyro()
    {
        if (SystemInfo.supportsGyroscope)
        {
            _gyro = Input.gyro;
            _gyro.enabled = true;
            return true;
        }
        return false;
    }

    private static Quaternion GyroToUnity(Quaternion q)
    {
        return new Quaternion(q.x, q.y, -q.z, -q.w);
    }
}
