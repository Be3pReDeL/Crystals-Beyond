using UnityEngine;

public class Set60FPS : MonoBehaviour
{
    private void Awake() => Application.targetFrameRate = 60;
}
