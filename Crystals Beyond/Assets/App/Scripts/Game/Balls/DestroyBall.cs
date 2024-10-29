using UnityEngine;

public class DestroyBall : MonoBehaviour
{
    private static Transform _spawnPoint; 
    private static string _SPAWNPOINTOBJECTNAME = "Spawn Point";

    private void Awake() 
    {
        _spawnPoint = GameObject.Find(_SPAWNPOINTOBJECTNAME).transform;
    } 

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerCircleSegment segment = collision.GetComponent<PlayerCircleSegment>();

        if (segment != null)
        {
            UpdateScores(segment);
            
            SpawnParticle(segment.Particle);

            VibrationController.Instance.Vibrate(VibrationController.VibrationType.medium);

            if (segment.ScoreValue < 0)
                Player.Instance.TakeDamage(10);

            Destroy(gameObject);
        }
    }

    private void UpdateScores(PlayerCircleSegment segment)
    {
        GameController.Instance.ScorePoints(segment.ScoreValue);
        if(GameController.Instance.CurrentGameMode == GameController.GameMode.levels)
            GameController.Instance.ScoreGoal();
    }

    private void SpawnParticle(GameObject particle) => Instantiate(particle, transform.position, Quaternion.identity, _spawnPoint);
}
