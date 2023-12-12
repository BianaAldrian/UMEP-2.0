using System.Collections;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.Networking;
using System.Net;
using System.Net.Sockets;

public class SetFloorUsers : MonoBehaviour
{
    private string serverIP;
    private string id_number = "";
    private string deviceUUID;

    private string previousBSSID=""; // Store the previous BSSID

    private float checkInterval = 1f;

    // Start is called before the first frame update
    void Start()
    {
        serverIP = PlayerPrefs.GetString("serverIP");
        id_number = PlayerPrefs.GetString("id_number");

        if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
        {
            Permission.RequestUserPermission(Permission.FineLocation);
        }

        deviceUUID = SystemInfo.deviceUniqueIdentifier;

        // Print the device UUID to the console
        Debug.Log("Device UUID: " + deviceUUID);

        // Start the coroutine for checking BSSID changes
        StartCoroutine(CheckBSSIDChanges());
    }

    IEnumerator CheckBSSIDChanges()
    {
        while (true)
        {
            yield return new WaitForSeconds(checkInterval);

            if (Permission.HasUserAuthorizedPermission(Permission.FineLocation))
            {
                using (var activity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity"))
                using (var wifiManager = activity.Call<AndroidJavaObject>("getSystemService", "wifi"))
                using (var wifiInfo = wifiManager.Call<AndroidJavaObject>("getConnectionInfo"))
                {
                    string currentBSSID = wifiInfo.Call<string>("getBSSID");

                    Debug.Log($"BSSID: {currentBSSID}");

                    /*
                    // Check if the current BSSID is different from the previous one
                    if (currentBSSID != previousBSSID)
                    {
                        previousBSSID = currentBSSID; // Update previousBSSID

                        // Perform the condition based on the current BSSID
                        if (currentBSSID == "00:eb:d8:7c:84:8c")
                        {
                            StartCoroutine(SetUserFloorCoroutine(10));
                        }
                        else if (currentBSSID == "00:eb:d8:c7:88:88")
                        {
                            StartCoroutine(SetUserFloorCoroutine(9));
                        }
                        else
                        {
                            Debug.Log("Invalid Router");
                        }
                    }
                    */

                    // Perform the condition based on the current BSSID
                    if (currentBSSID == "00:eb:d8:7c:84:8c")
                    {
                        StartCoroutine(SetUserFloorCoroutine(10));
                    }
                    else if (currentBSSID == "00:eb:d8:c7:88:88")
                    {
                        StartCoroutine(SetUserFloorCoroutine(9));   
                    }
                    else
                    {
                        Debug.Log("Invalid Router");
                    }
                }
            }
            else
            {
                Debug.Log("Fine location permission is not granted.");
            }
        }
    }

    IEnumerator SetUserFloorCoroutine(int floor_num)
    {
        WWWForm form = new WWWForm();
        form.AddField("deviceUUID", deviceUUID);
        form.AddField("id_number", id_number);
        form.AddField("floor_num", floor_num);

        UnityWebRequest webRequest = UnityWebRequest.Post($"http://{serverIP}/UMEP/setUsers_floor.php", form);
        yield return webRequest.SendWebRequest();

        if (webRequest.result != UnityWebRequest.Result.Success)
        {
            Debug.LogWarning($"HTTP request failed. Error: {webRequest.error}");
            Debug.LogWarning($"Server IP: {serverIP}");

            // Handle the error, display a message, or take appropriate action

        }
        else
        {
            string response = webRequest.downloadHandler.text;
            Debug.Log($"Server response: {response}");
            /*
            if (response == "exist")
            {
                Debug.Log("ipAddress already exist");
            }
            */
            if (response == "update_success")
            {
                Debug.Log("updated successfully");
            }

            if (response == "insert_success")
            {
                Debug.Log("inserted successfully");
            }
        }
    }

    void ConnectToStrongestWifi()
    {
        AndroidJavaClass unityPlayerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject unityActivity = unityPlayerClass.GetStatic<AndroidJavaObject>("currentActivity");

        AndroidJavaObject wifiManager = unityActivity.Call<AndroidJavaObject>("getSystemService", "wifi");

        // Start a Wi-Fi scan
        wifiManager.Call("startScan");

        // Get the scan results
        AndroidJavaObject scanResults = wifiManager.Call<AndroidJavaObject>("getScanResults");

        // Get the list of scan results
        AndroidJavaObject resultList = scanResults.Call<AndroidJavaObject>("getResultList");

        int numResults = resultList.Call<int>("size");

        int maxSignalStrength = int.MinValue;
        string strongestWifiSSID = "";

        // Iterate through the scan results to find the strongest Wi-Fi network
        for (int i = 0; i < numResults; i++)
        {
            AndroidJavaObject scanResult = resultList.Call<AndroidJavaObject>("get", i);
            string ssid = scanResult.Call<string>("SSID");
            int signalStrength = scanResult.Call<int>("level");

            if (signalStrength > maxSignalStrength)
            {
                maxSignalStrength = signalStrength;
                strongestWifiSSID = ssid;
            }
        }

        // Connect to the strongest Wi-Fi network
        if (!string.IsNullOrEmpty(strongestWifiSSID))
        {
            string strongestWifiPassword = "your_wifi_password";
            wifiManager.Call("enableNetwork", strongestWifiSSID, strongestWifiPassword, true);
        }
        else
        {
            Debug.LogError("No available Wi-Fi networks found.");
        }
    }

}
