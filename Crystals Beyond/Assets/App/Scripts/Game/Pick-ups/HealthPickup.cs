using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    [SerializeField] private int healthAmount = 20;         // Количество здоровья, которое восстановит предмет
    [SerializeField] private ParticleSystem pickupEffect;   // Префаб для эффекта при подборе

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ball"))  // Проверяем, что игрок касается предмета
        {
            Player.Instance.Heal(healthAmount);  // Увеличиваем здоровье игрока

            if (pickupEffect != null)
            {
                ParticleSystem effect = Instantiate(pickupEffect, transform.position, Quaternion.identity);
            }

            Destroy(gameObject);  // Удаляем предмет после подбора
        }
    }
}
