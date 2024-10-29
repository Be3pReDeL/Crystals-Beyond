using UnityEngine;
using System.Collections;
using Unity.Advertisement.IosSupport.Components;

namespace Unity.Advertisement.IosSupport.Samples
{
    public class ContextScreenManager : MonoBehaviour
    {
        [SerializeField] private ContextScreenView _contextScreenView;

        private const string _PLAYERPREFSKEY = "GIdatLPL";

        private void Start()
        {
#if UNITY_IOS
            ATTrackingStatusBinding.AuthorizationTrackingStatus status = ATTrackingStatusBinding.GetAuthorizationTrackingStatus();
 
            if (status == ATTrackingStatusBinding.AuthorizationTrackingStatus.NOT_DETERMINED || 
            status == ATTrackingStatusBinding.AuthorizationTrackingStatus.DENIED || 
            status == ATTrackingStatusBinding.AuthorizationTrackingStatus.RESTRICTED)
                _contextScreenView.SentTrackingAuthorizationRequest?.AddListener(StartStartGIDATLPLCoroutine);
            else
            {
                PlayerPrefs.SetInt(_PLAYERPREFSKEY, 1);

                LoadSceneButton.LoadRelativeScene(1);
            }
#endif
        }

        private IEnumerator StartGIDATLPL()
        {
#if UNITY_IOS
            ATTrackingStatusBinding.AuthorizationTrackingStatus status = ATTrackingStatusBinding.GetAuthorizationTrackingStatus();
 
            while (status == ATTrackingStatusBinding.AuthorizationTrackingStatus.NOT_DETERMINED)
            {
                status = ATTrackingStatusBinding.GetAuthorizationTrackingStatus();

                if (status == ATTrackingStatusBinding.AuthorizationTrackingStatus.AUTHORIZED)
                    PlayerPrefs.SetInt(_PLAYERPREFSKEY, 1);

                yield return null;
            }
#endif
            LoadSceneButton.LoadRelativeScene(1);

            yield return null;
        }

        private void StartStartGIDATLPLCoroutine() => StartCoroutine(StartGIDATLPL());

        private void OnDisable() => _contextScreenView.SentTrackingAuthorizationRequest?.RemoveListener(StartStartGIDATLPLCoroutine);
    }
}
