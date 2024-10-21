using UnityEngine;

public class DestroyBall : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerCircleSegment segment = collision.GetComponent<PlayerCircleSegment>();

        if (segment != null)
        {
            UpdateScores(segment);
            
            SpawnParticle(segment.Particle);

            PrefabSpawner.Instance.DeleteBallFromList(gameObject);

            Destroy(gameObject);
        }
    }

    private void UpdateScores(PlayerCircleSegment segment)
    {
        if(GameController.Instance.CurrentGameMode == GameController.GameMode.endless)
            GameController.Instance.ScorePoints(segment.ScoreValue);
        else if (segment.ScoreValue > 0)
            GameController.Instance.ScoreGoal();
    }

    private void SpawnParticle(GameObject particle) => Instantiate(particle, transform.position, Quaternion.identity);
}
