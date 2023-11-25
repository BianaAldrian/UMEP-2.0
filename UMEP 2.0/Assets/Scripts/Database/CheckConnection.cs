using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class CheckConnection : MonoBehaviour
{
    public string IP = "192.168.10.106";
    public bool isConnected;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(CheckServerConnection());
    }

    IEnumerator CheckServerConnection()
    {
        UnityWebRequest webRequest = UnityWebRequest.Get($"http://{IP}/UMEP/test.php");

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
        isConnected = true;
        // Show results as text
        Debug.Log("Request successful:\n" + webRequest.downloadHandler.text);

        // Or retrieve results as binary data
        byte[] results = webRequest.downloadHandler.data;
    }

    void HandleRequestError(UnityWebRequest webRequest)
    {
        isConnected = false;
        Debug.LogError("Request failed:\n" + webRequest.error);

        // You might want to handle the error more specifically or inform the user
    }
}
