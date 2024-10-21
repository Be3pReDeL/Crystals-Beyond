using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BallMovement : MonoBehaviour
{
    private Rigidbody2D _rb;
    private float _currentSpeed;  // Текущая скорость
    private float _speedIncreaseRate;  // Скорость прироста

    private void Update()
    {
        // Постепенно увеличиваем скорость
        _currentSpeed += _speedIncreaseRate * Time.deltaTime;

        // Нормализуем направление движения, чтобы поддерживать постоянную скорость
        _rb.velocity = _rb.velocity.normalized * _currentSpeed;
    }

    public void Setup(Rigidbody2D rb, float initialSpeed, float speedIncreaseRate)
    {
        _rb = rb;
        _currentSpeed = initialSpeed;
        _speedIncreaseRate = speedIncreaseRate;
    }

    // Обработка столкновений с изменением направления
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Меняем направление движения, сохраняя текущую скорость
        Vector2 newDirection = Vector2.Reflect(_rb.velocity.normalized, collision.contacts[0].normal);
        _rb.velocity = newDirection * _currentSpeed;
    }
}
