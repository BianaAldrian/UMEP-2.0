using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking; // Add this line
using System.Net;

public class IPGeoLocation : MonoBehaviour
{
    public Text locationText;

    void Start()
    {
        StartCoroutine(GetIP());
    }

    IEnumerator GetIP()
    {
        // Fetching the public IP address of the device
        string ipAddress = "";
        using (WebClient client = new WebClient())
        {
            ipAddress = client.DownloadString("https://api64.ipify.org?format=text");
        }

        Debug.Log($"Fetched IP Address: {ipAddress}");

        // Set the fetched IP address to the UI Text element
        locationText.text = $"IP Address: {ipAddress}";

        yield return null; // You can omit this line if not needed
    }
}

    [System.Serializable]
public class IPInfo
{
    public string ip;
    public string city;
    public string region;
    public string country;
    // Add more fields as needed
}
