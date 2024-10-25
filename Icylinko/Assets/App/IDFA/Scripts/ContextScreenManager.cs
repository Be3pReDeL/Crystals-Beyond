using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Advertisement.IosSupport.Components;
using System.Collections;

namespace Unity.Advertisement.IosSupport.Samples
{
    public class ContextScreenManager : MonoBehaviour
    {
        [SerializeField] private ContextScreenView _contextScreen;

        private const string _PLAYERPREFSKEY = "GIDATLPL";

        private void Start()
        {
#if UNITY_IOS
            var status = ATTrackingStatusBinding.GetAuthorizationTrackingStatus();
 
            if (status == ATTrackingStatusBinding.AuthorizationTrackingStatus.NOT_DETERMINED)
                _contextScreen.RequestAuthorizationTracking();
#else
            Debug.Log("Unity iOS Support: App Tracking Transparency status not checked, because the platform is not iOS.");
#endif
            StartCoroutine(StartGIDATLPL());
        }

        private IEnumerator StartGIDATLPL()
        {
#if UNITY_IOS && !UNITY_EDITOR
            var status = ATTrackingStatusBinding.GetAuthorizationTrackingStatus();
 
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
    }
}