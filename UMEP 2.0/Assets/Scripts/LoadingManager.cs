using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class LoadingManager : MonoBehaviour
{
    // IP address for server connection
    private string IP;
    // Images for the transition effect
    public Image firstImage;
    public Image secondImage;
    public Image thirdImage;
    // Duration of the fade effect
    public float fadeDuration = 1.0f;
    // Name of the scene to load after the transition
    public string sceneName;

    // Android dialog for displaying messages and alerts
    private AndroidDialog androidDialog;
    // Reference to the current Android activity
    private AndroidJavaObject currentActivity;

    void Start()
    {
        IP = PlayerPrefs.GetString("IP");
        // Initialize the Android dialog and check Wi-Fi status
        InitializeAndroidDialog();
        CheckAndShowWifiDialog();
    }

    #region Wi-Fi Dialog
    void InitializeAndroidDialog()
    {
        // Get the current Android activity
        using (var unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        }

        // Initialize the AndroidDialog instance
        androidDialog = gameObject.AddComponent<AndroidDialog>();
    }

    void CheckAndShowWifiDialog()
    {
        Debug.Log("Checking and showing Wi-Fi dialog...");
        InitializeAndroidDialog();

        // Check if Wi-Fi is available
        if (Application.internetReachability != NetworkReachability.ReachableViaLocalAreaNetwork)
        {
            // Show Wi-Fi dialog if Wi-Fi is off
            androidDialog.ShowAlertDialog("Wi-Fi", "Turn on your wifi first!", "Try Again", "Close", CheckWifiAgain, OnCloseClick);
            androidDialog.HideEditText();
        }
        else
        {
            if (!string.IsNullOrEmpty(IP) && IP != "")
            {
                StartCoroutine(CheckServerConnection());
            }
            else
            {
                // Show an error dialog if the IP is empty
                androidDialog.ShowAlertDialog("Wi-Fi", "Empty IP Address, Connect to Main Router!", "Try Again", "Close", CheckWifiAgain, OnCloseClick);
                // Show the EditText in the dialog
                androidDialog.ShowEditText();

                // Get the value from the EditText
                IP = androidDialog.GetEditTextValue();
                Debug.Log($"Entered text: {IP}");
            }
        }
    }

    void CloseWifiDialog()
    {
        // Close the Wi-Fi dialog
        androidDialog.CloseAlertDialog();
    }

    void CheckWifiAgain()
    {
        Debug.Log("Checking Wi-Fi again...");
        // Try again button clicked, check Wi-Fi status again
        CheckAndShowWifiDialog();
    }

    void OnCloseClick()
    {
        // Close button clicked, close the application on Android
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
        // Create a UnityWebRequest to check server connection
        UnityWebRequest webRequest = UnityWebRequest.Get($"http://{IP}/UMEP/test.php");

        // Send the web request and wait for the response
        yield return webRequest.SendWebRequest();

        // Check if the request was successful or failed
        if (webRequest.result != UnityWebRequest.Result.Success)
        {
            // Handle the error if the request fails
            HandleRequestError(webRequest);
        }
        else
        {
            // Handle the success if the request is successful
            HandleRequestSuccess(webRequest);
        }
    }

    void HandleRequestSuccess(UnityWebRequest webRequest)
    {
        // Log the successful response
        Debug.Log("Request successful:\n" + webRequest.downloadHandler.text);

        // Save the IP address in PlayerPrefs for future use
        PlayerPrefs.SetString("IP", IP);
        PlayerPrefs.Save();

        // Close the Wi-Fi dialog and initiate the transition effect
        CloseWifiDialog();
        StartCoroutine(TransitionImages());
    }

    void HandleRequestError(UnityWebRequest webRequest)
    {
        InitializeAndroidDialog();

        // Log the request failure
        Debug.LogError("Request failed:\n" + webRequest.error);

        // Delete the saved IP address in PlayerPrefs
        PlayerPrefs.DeleteKey("IP");

        // Show an error dialog if the connection to the main router is invalid
        androidDialog.ShowAlertDialog("Wi-Fi", "Invalid connection, Connect to Main Router!", "Try Again", "Close", CheckConnectionAgain, OnCloseClick);
        // Show the EditText in the dialog
        androidDialog.ShowEditText();
    }

    void CheckConnectionAgain()
    {
        // Try again button clicked, check connection to the database status again
        StartCoroutine(CheckServerConnection());
    }
    #endregion

    #region Transition
    private IEnumerator TransitionImages()
    {
        // Optional delay before fading.
        yield return new WaitForSeconds(1);

        if (firstImage != null && secondImage != null && thirdImage != null)
        {
            // Fade out the first image
            for (float t = 0; t < 1; t += Time.deltaTime / fadeDuration)
            {
                firstImage.color = new Color(1, 1, 1, 1 - t);
                secondImage.color = new Color(1, 1, 1, 1 - t);
                yield return null;
            }

            // Hide the first image and show the second image
            firstImage.gameObject.SetActive(false);
            secondImage.gameObject.SetActive(false);
            thirdImage.gameObject.SetActive(true);

            // Fade in the third image
            for (float t = 0; t < 1; t += Time.deltaTime / fadeDuration)
            {
                thirdImage.color = new Color(1, 1, 1, t);
                yield return null;
            }

            // Fade out the third image
            for (float t = 0; t < 1; t += Time.deltaTime / fadeDuration)
            {
                thirdImage.color = new Color(1, 1, 1, 1 - t);
                yield return null;
            }

            // Load the main scene
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogError("Image references not set in the editor.");
        }
    }
    #endregion
}
