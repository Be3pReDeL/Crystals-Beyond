using UnityEngine;

public class ScreenController : MonoBehaviour
{
    [SerializeField] private UIAnimator[] _UIAnimators;

    private void OnEnable() 
    {
        foreach (var animator in _UIAnimators) 
            animator.Appear();
    }

    public void CloseScreen() 
    {
        foreach (var animator in _UIAnimators) 
            animator.Disappear();
    }
}
