using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using UnityEngine.Android;

public class LoadingManager : MonoBehaviour
{
    private string serverIP = "192.168.1.5";
    public Image firstImage;
    public Image secondImage;
    public Image thirdImage;
    public float fadeDuration;
    public string sceneName;

    private AndroidDialog androidDialog;
    private AndroidJavaObject currentActivity;

    void Start()
    {
        InitializeAndroidDialog();
        CheckAndShowWifiDialog();
    }

    #region Wi-Fi Dialog
    void InitializeAndroidDialog()
    {
        using (var unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        }

        androidDialog = GetComponent<AndroidDialog>();

        if (androidDialog == null)
        {
            androidDialog = gameObject.AddComponent<AndroidDialog>();
        }
    }

    void CheckAndShowWifiDialog()
    {
        Debug.Log("Checking and showing Wi-Fi dialog...");

        if (Application.internetReachability != NetworkReachability.ReachableViaLocalAreaNetwork)
        {
            androidDialog.ShowAlertDialog("Wi-Fi", "Turn on your wifi first!", "Try Again", "Close", CheckWifiAgain, OnCloseClick);
        }
        else
        {
            if (!string.IsNullOrEmpty(serverIP) && serverIP != "")
            {
                StartCoroutine(CheckServerConnection());
            }
            else
            {
                androidDialog.ShowAlertDialog("Wi-Fi", "Empty IP Address, Connect to Main Router!", "Try Again", "Close", CheckWifiAgain, OnCloseClick);
            }
        }
    }

    void CloseWifiDialog()
    {
        androidDialog.CloseAlertDialog();
    }

    void CheckWifiAgain()
    {
        InitializeAndroidDialog();
        Debug.Log("Checking Wi-Fi again...");
        CheckAndShowWifiDialog();
    }

    void OnCloseClick()
    {
#if UNITY_ANDROID
        if (currentActivity != null)
        {
            currentActivity.Call("finish");
        }
#endif
    }
    #endregion

    #region Network Requests
    IEnumerator CheckServerConnection()
    {
        Debug.Log($"Checking server connection to IP: {serverIP}");

        UnityWebRequest webRequest = UnityWebRequest.Get($"http://{serverIP}/UMEP/test.php");

        yield return webRequest.SendWebRequest();

        if (webRequest.result != UnityWebRequest.Result.Success)
        {
            HandleRequestError(webRequest);
        }
        else
        {
            HandleRequestSuccess(webRequest);
        }
    }

    void HandleRequestSuccess(UnityWebRequest webRequest)
    {
        Debug.Log("Request successful:\n" + webRequest.downloadHandler.text);

        PlayerPrefs.SetString("serverIP", serverIP);
        PlayerPrefs.Save();

        CloseWifiDialog();

        Permission.RequestUserPermission(Permission.FineLocation);
        if (Permission.HasUserAuthorizedPermission(Permission.FineLocation))
        {
            StartCoroutine(CheckLocationStatus());
        }
        else
        {
            androidDialog.ShowAlertDialog("Location", "Location permission is required. Please grant the permission.", "Try Again", "Close", CheckWifiAgain, OnCloseClick);
        }
    }

    IEnumerator CheckLocationStatus()
    {
        if (Input.location.isEnabledByUser)
        {
            StartCoroutine(TransitionImages());
        }
        else
        {
            androidDialog.ShowAlertDialog("Location", "Turn on your location first!", "Enable Location", "Close", EnableLocation, OnCloseClick);
        }

        yield return null;
    }

    void EnableLocation()
    {
        Application.OpenURL("app-settings:");
        StartCoroutine(CheckLocationStatus());
    }

    void HandleRequestError(UnityWebRequest webRequest)
    {
        InitializeAndroidDialog();

        Debug.LogError("Request failed:\n" + webRequest.error);

        PlayerPrefs.DeleteKey("serverIP");

        androidDialog.ShowAlertDialog("Wi-Fi", "Invalid connection, Connect to Main Router!", "Try Again", "Close", CheckConnectionAgain, OnCloseClick);
    }

    void CheckConnectionAgain()
    {
        StartCoroutine(CheckServerConnection());
    }
    #endregion

    #region Transition
    private IEnumerator TransitionImages()
    {
        yield return new WaitForSeconds(1);

        if (firstImage != null && secondImage != null && thirdImage != null)
        {
            for (float t = 0; t < 1; t += Time.deltaTime / fadeDuration)
            {
                firstImage.color = new Color(1, 1, 1, 1 - t);
                secondImage.color = new Color(1, 1, 1, 1 - t);
                yield return null;
            }

            firstImage.gameObject.SetActive(false);
            secondImage.gameObject.SetActive(false);
            thirdImage.gameObject.SetActive(true);

            for (float t = 0; t < 1; t += Time.deltaTime / fadeDuration)
            {
                thirdImage.color = new Color(1, 1, 1, t);
                yield return null;
            }

            for (float t = 0; t < 1; t += Time.deltaTime / fadeDuration)
            {
                thirdImage.color = new Color(1, 1, 1, 1 - t);
                yield return null;
            }

            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogError("Image references not set in the editor.");
        }
    }
    #endregion
}