using UnityEngine;
using UnityEngine.Events;

namespace Unity.Advertisement.IosSupport.Components
{
    public sealed class ContextScreenView : MonoBehaviour
    {
        public UnityEvent SentTrackingAuthorizationRequest;

        private void Awake() 
        {
            if (SentTrackingAuthorizationRequest == null)
                SentTrackingAuthorizationRequest = new UnityEvent();
        }

        private void Start() 
        {
            RequestAuthorizationTracking();
        }

        public void RequestAuthorizationTracking()
        {
#if UNITY_IOS
            ATTrackingStatusBinding.RequestAuthorizationTracking();

            SentTrackingAuthorizationRequest?.Invoke();
#endif
        }
    }
}
