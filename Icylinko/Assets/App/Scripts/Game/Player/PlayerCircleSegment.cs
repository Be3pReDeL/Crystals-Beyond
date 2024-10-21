using UnityEngine;

public class PlayerCircleSegment : MonoBehaviour
{
    [SerializeField] private int _scoreValue; // Количество очков, которое добавляется или вычитается
    [SerializeField] private GameObject _particle; // Префаб частиц, которые будут спавниться при столкновении с сегментом

    public int ScoreValue => _scoreValue; // Свойство для доступа к значению очков
    public GameObject Particle => _particle; // Свойство для доступа к префабу частиц
}
