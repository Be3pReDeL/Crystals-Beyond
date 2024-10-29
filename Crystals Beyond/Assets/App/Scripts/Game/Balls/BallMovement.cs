using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(AudioSource))]
public class BallMovement : MonoBehaviour
{
    private Rigidbody2D _rb;
    
    [SerializeField] private float _initialSpeed = 5f;
    [SerializeField] private float _speedIncreaseRate = 0.5f;

    private float _currentSpeed;
    private AudioSource _audioSource;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _audioSource = GetComponent<AudioSource>();
        
        InitializeMovement();
    }

    private void Update() => IncreaseSpeed();

    // Инициализируем начальное состояние движения
    private void InitializeMovement()
    {
        _currentSpeed = _initialSpeed;
        _rb.velocity = _rb.velocity.normalized * _currentSpeed;
    }

    // Увеличиваем скорость плавно
    private void IncreaseSpeed()
    {
        _currentSpeed += _speedIncreaseRate * Time.deltaTime;
        _rb.velocity = _rb.velocity.normalized * _currentSpeed;
    }

    // Метод для обработки звука столкновения
    private void PlayCollisionSound()
    {
        if (_audioSource != null)
            _audioSource.Play();
    }

    // Обработка столкновений с отражением направления
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.contacts.Length > 0)
        {
            Vector2 newDirection = Vector2.Reflect(_rb.velocity.normalized, collision.contacts[0].normal);
            _rb.velocity = newDirection * _currentSpeed;
            
            PlayCollisionSound();
        }
    }

    public void Setup(float initialSpeed, float speedIncreaseRate)
    {
        _initialSpeed = initialSpeed;
        _speedIncreaseRate = speedIncreaseRate;
        InitializeMovement();
    }
}
