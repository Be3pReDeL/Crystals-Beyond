using UnityEngine;

public class HealthPickupSpawner : MonoBehaviour
{
    [SerializeField] private GameObject healthPickupPrefab;      // Префаб для HealthPickup
    [SerializeField] private float spawnInterval = 5f;           // Интервал спавна предметов
    [SerializeField] private GameObject spawnAreaObject;         // Ссылка на объект, определяющий зону спавна
    [SerializeField] private Vector3 spawnAreaSize = new Vector3(10f, 0f, 10f); // Размер зоны спавна, настраиваемый в инспекторе

    private void Start()
    {
        if (spawnAreaObject != null)
        {
            // Устанавливаем размер зоны спавна на основе размеров spawnAreaObject
            spawnAreaSize = spawnAreaObject.transform.localScale;
        }

        InvokeRepeating(nameof(SpawnPickup), spawnInterval, spawnInterval); // Запускаем спавн по таймеру
    }

    private void SpawnPickup()
    {
        if (healthPickupPrefab == null) return;

        // Генерируем случайную позицию внутри зоны
        Vector3 randomPosition = new Vector3(
            Random.Range(-spawnAreaSize.x / 2, spawnAreaSize.x / 2),
            0,
            Random.Range(-spawnAreaSize.z / 2, spawnAreaSize.z / 2)
        );

        // Спавним предмет в пределах зоны
        Instantiate(healthPickupPrefab, transform.position + randomPosition, Quaternion.identity);
    }

    // Визуализация зоны спавна в редакторе
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(spawnAreaObject != null ? spawnAreaObject.transform.position : transform.position, spawnAreaSize);
    }
}
