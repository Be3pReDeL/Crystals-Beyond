using UnityEngine;

public class GyroRotationController : MonoBehaviour
{
    private bool _gyroEnabled;
    private Gyroscope _gyro;
    private float _initialZRotation;

    private void Start()
    {
        // Включаем гироскоп, если он доступен
        _gyroEnabled = EnableGyro();

        // Сохраняем начальный угол по оси Z
        _initialZRotation = transform.localEulerAngles.z;
    }

    private void Update()
    {
        if (_gyroEnabled)
        {
            // Преобразуем данные гироскопа в углы Эйлера
            Quaternion deviceRotation = GyroToUnity(_gyro.attitude);
            Vector3 eulerAngles = deviceRotation.eulerAngles;

            // Получаем угол по оси Z
            float zRotation = eulerAngles.z;

            //Debug.Log(zRotation);

            // Плавно интерполируем только вращение по оси Z
            float smoothZRotation = Mathf.LerpAngle(transform.localEulerAngles.z, _initialZRotation + zRotation, Time.deltaTime * 5f);

            // Применяем вращение только по оси Z
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, smoothZRotation);
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
