using UnityEngine;

public class RotateObjectWithGyro : MonoBehaviour
{
    private enum RotateAxis { x, y, z }
    [SerializeField] private RotateAxis _axis;
    [SerializeField] private float _multiplier = 1f;

    private bool _gyroEnabled;
    private Gyroscope _gyro;
    private Vector3 _initialRotation;

    private void Start()
    {
        // Включаем гироскоп, если он доступен
        _gyroEnabled = EnableGyro();

        // Сохраняем начальные углы вращения по всем осям
        _initialRotation = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, transform.localEulerAngles.z);
    }

    private void Update()
    {
        if (_gyroEnabled)
        {
            // Преобразуем данные гироскопа в углы Эйлера
            Quaternion deviceRotation = GyroToUnity(_gyro.attitude);
            Vector3 eulerAngles = deviceRotation.eulerAngles;

            switch (_axis)
            {
                case RotateAxis.x:
                    float xRotation = eulerAngles.x * _multiplier; // Умножаем на _multiplier
                    float smoothXRotation = Mathf.LerpAngle(transform.localEulerAngles.x, _initialRotation.x + xRotation, Time.deltaTime * 5f);
                    transform.localEulerAngles = new Vector3(smoothXRotation, transform.localEulerAngles.y, transform.localEulerAngles.z);
                    break;

                case RotateAxis.y:
                    float yRotation = eulerAngles.y * _multiplier; // Умножаем на _multiplier
                    float smoothYRotation = Mathf.LerpAngle(transform.localEulerAngles.y, _initialRotation.y + yRotation, Time.deltaTime * 5f);
                    transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, smoothYRotation, transform.localEulerAngles.z);
                    break;

                case RotateAxis.z:
                    float zRotation = eulerAngles.z * _multiplier; // Умножаем на _multiplier
                    float smoothZRotation = Mathf.LerpAngle(transform.localEulerAngles.z, _initialRotation.z + zRotation, Time.deltaTime * 5f);
                    transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, smoothZRotation);
                    break;
            }
        }
    }

    private bool EnableGyro()
    {
        // Проверяем, поддерживает ли устройство гироскоп
        if (SystemInfo.supportsGyroscope)
        {
            _gyro = Input.gyro;
            _gyro.enabled = true;  // Включаем гироскоп
            return true;
        }
        return false;
    }

    // Преобразуем данные из системы координат устройства в систему координат Unity
    private static Quaternion GyroToUnity(Quaternion q)
    {
        return new Quaternion(q.x, q.y, -q.z, -q.w);
    }
}
