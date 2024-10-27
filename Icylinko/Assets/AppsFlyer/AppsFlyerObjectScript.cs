using UnityEngine;
using System.Collections.Generic;
using AppsFlyerSDK;

public class AppsFlyerObjectScript : MonoBehaviour, IAppsFlyerConversionData
{
    [SerializeField] private string _devKey;
    [SerializeField] private string _appID;
    [SerializeField] private bool _getConversionData;

    private const string _PLAYERPREFSKEY = "BOdatLPN";

    private void Start()
    {
        AppsFlyer.setIsDebug(false);
        AppsFlyer.initSDK(_devKey, _appID, _getConversionData ? this : null);

        AppsFlyer.startSDK();
    }

    public void onConversionDataSuccess(string popoxc)
    {
        AppsFlyer.AFLog("didReceiveConversionData", popoxc);
        Dictionary<string, object> convData = AppsFlyer.CallbackStringToDictionary(popoxc);

        string aghsd = "";

        if (convData.ContainsKey("campaign"))
        {
            object conv;

            if (convData.TryGetValue("campaign", out conv))
            {
                string[] list = conv.ToString().Split('_');

                if (list.Length > 0)
                {
                    aghsd = "&";

                    for (int a = 0; a < list.Length; a++)
                    {
                        aghsd += string.Format("sub{0}={1}", (a + 1), list[a]);
                        
                        if (a < list.Length - 1)
                            aghsd += "&";
                    }
                }
            }
        }

        PlayerPrefs.SetString(_PLAYERPREFSKEY, aghsd);
    }

    public void onConversionDataFail(string error)
    {
        AppsFlyer.AFLog("didReceiveConversionDataWithError", error);
        PlayerPrefs.SetString(_PLAYERPREFSKEY, "");
    }

    public void onAppOpenAttribution(string attributionData)
    {
        AppsFlyer.AFLog("onAppOpenAttribution", attributionData);
        PlayerPrefs.SetString(_PLAYERPREFSKEY, "");
    }

    public void onAppOpenAttributionFailure(string error)
    {
        AppsFlyer.AFLog("onAppOpenAttributionFailure", error);
        PlayerPrefs.SetString(_PLAYERPREFSKEY, "");
    }
}
