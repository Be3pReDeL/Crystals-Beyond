using UnityEngine;

public class DestroyBall : MonoBehaviour
{
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private string _spawnPointTag = "SpawnPoint";

    private void Awake()
    {
        if (_spawnPoint == null)
        {
            GameObject spawnObject = GameObject.FindWithTag(_spawnPointTag);
            if (spawnObject != null)
                _spawnPoint = spawnObject.transform;
            else
                Debug.LogWarning("Spawn point not found. Ensure an object with tag 'SpawnPoint' is present in the scene.");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerCircleSegment segment = collision.GetComponent<PlayerCircleSegment>();

        if (segment != null)
        {
            UpdateScores(segment);
            SpawnParticle(segment.Particle);
            HandleVibration();

            if (segment.ScoreValue < 0)
                Player.Instance?.TakeDamage(10);

            Destroy(gameObject);
        }
    }

    private void UpdateScores(PlayerCircleSegment segment)
    {
        if (GameController.Instance != null)
        {
            GameController.Instance.ScorePoints(segment.ScoreValue);
            if (GameController.Instance.CurrentGameMode == GameController.GameMode.Levels)
                GameController.Instance.ScoreGoal();
        }
    }

    private void SpawnParticle(GameObject particle)
    {
        if (_spawnPoint != null)
            Instantiate(particle, transform.position, Quaternion.identity, _spawnPoint);
        else
            Instantiate(particle, transform.position, Quaternion.identity);
    }

    private void HandleVibration()
    {
        if (VibrationController.Instance != null)
            VibrationController.Instance.Vibrate(VibrationController.VibrationType.medium);
    }
}
