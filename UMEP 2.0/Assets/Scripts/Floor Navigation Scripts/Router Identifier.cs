using UnityEngine;
using UnityEngine.Android;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RouterIdentifier : MonoBehaviour
{
    public Button Interactive;

    // Start is called before the first frame update
    void Start()
    {
        if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
        {
            Permission.RequestUserPermission(Permission.FineLocation);
        }

        //Interactive.onClick.AddListener(Identify_Router);
        SceneManager.LoadScene("10th floor");
    }

    void Identify_Router()
    {
        if (Permission.HasUserAuthorizedPermission(Permission.FineLocation))
        {
            using var activity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
            using var wifiManager = activity.Call<AndroidJavaObject>("getSystemService", "wifi");
            using var wifiInfo = wifiManager.Call<AndroidJavaObject>("getConnectionInfo");
            string bssid = wifiInfo.Call<string>("getBSSID");

            Debug.Log($"BSSID: {bssid}");

            if (bssid == "00:eb:d8:7c:84:8c")
            {
                SceneManager.LoadScene("10th floor");
            }
            else if (bssid == "00:eb:d8:c7:88:88")
            {
                SceneManager.LoadScene("9th floor");
            }
            else
            {
                Debug.Log("Invalid Router");
            }
        }
        else
        {
            Debug.Log("Fine location permission is not granted.");
        }
    }
}
