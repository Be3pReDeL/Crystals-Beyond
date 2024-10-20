using UnityEngine;

public class ScreenController : MonoBehaviour
{
    [SerializeField] private UIAnimator[] _UIAnimators;

    public void CloseScreen() 
    {
        foreach (var animator in _UIAnimators) 
            animator.Disappear(gameObject);
    }
}
