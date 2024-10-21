using UnityEngine;

public class SmoothCameraFollow : MonoBehaviour
{
    [SerializeField] private Transform _target;  // Объект, за которым следует камера (только поворот)
    [SerializeField] private float _smoothSpeed = 0.125f;  // Скорость плавного следования камеры

    private Quaternion _targetRotation;  // Конечный поворот камеры

    private void LateUpdate()
    {
        // Берем поворот объекта-цели
        _targetRotation = _target.rotation;

        // Плавно интерполируем поворот камеры, чтобы повторить поворот объекта
        transform.rotation = Quaternion.Slerp(transform.rotation, _targetRotation, _smoothSpeed);
    }
}
