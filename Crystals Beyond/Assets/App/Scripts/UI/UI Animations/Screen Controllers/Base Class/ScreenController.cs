using UnityEngine;

[AddComponentMenu("UI/Animations/Screen Controller")]
public class ScreenController : MonoBehaviour
{
    [SerializeField] private UIAnimator[] _UIAnimators;

    public void CloseScreen() 
    {
        foreach (var animator in _UIAnimators) 
            if(animator != null && animator.gameObject.activeSelf)
                animator.Disappear(gameObject);
    }
}
