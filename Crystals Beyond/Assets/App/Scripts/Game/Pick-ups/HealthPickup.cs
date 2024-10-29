using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    [SerializeField] private int healthAmount = 20;         // Количество здоровья, которое восстановит предмет
    [SerializeField] private ParticleSystem pickupEffect;   // Префаб для эффекта при подборе

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ball"))  // Проверяем, что объект с тегом "Ball" касается предмета
        {
            Player.Instance?.Heal(healthAmount);  // Увеличиваем здоровье игрока, если Player.Instance не равен null

            if (pickupEffect != null)
            {
                Instantiate(pickupEffect, transform.position, Quaternion.identity);  // Создаем эффект подбора
            }

            Destroy(gameObject);  // Удаляем предмет после подбора
        }
    }
}
