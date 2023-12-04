using System.Collections;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.Networking;

public class SetFloorUsers : MonoBehaviour
{
    public CheckConnection checkConnection; // Calling other script to connect
    public string id_number;

    private string previousBSSID; // Store the previous BSSID

    // Start is called before the first frame update
    void Start()
    {
        if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
        {
            Permission.RequestUserPermission(Permission.FineLocation);
        }
    }

    // Update is called once per frame
    void Update()
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
                SetUserFloor(10);
            }
            else if (bssid == "00:eb:d8:c7:88:88")
            {
                SetUserFloor(9);
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

    void SetUserFloor(int floor_num)
    {
        if (checkConnection == null)
        {
            Debug.LogError("CheckConnection is null");
            return;
        }

        bool isConnected = checkConnection.isConnected;
        string serverIP = checkConnection.IP;

        if (isConnected)
        {
            Debug.Log("Connected");
            StartCoroutine(SetUserFloorCoroutine(serverIP, floor_num));
        }
    }

    IEnumerator SetUserFloorCoroutine(string serverIP, int floor_num)
    {
        WWWForm form = new WWWForm();
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
        }
    }
}
