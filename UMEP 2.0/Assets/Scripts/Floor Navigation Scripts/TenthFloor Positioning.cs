using UnityEngine;
using UnityEngine.Android;
using System.Collections.Generic;
using System.Collections;

public class TenthFloorPositioning : MonoBehaviour
{
    private string MACValue;
    string RSSIValue;
    private int previousRssi = -100;
    private string direction;
    private int distance;

    private const int NumReadings = 5;
    private List<int> rssiReadings = new List<int>();
    private List<int> rssiBuffer = new List<int>();
    private int bufferSize = 1;
    private const int CalibrationFactor = 5;

    private bool isCheckingDirection = true;
    private float checkInterval = 1f;

    void Start()
    {
        if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
        {
            Permission.RequestUserPermission(Permission.FineLocation);
        }

        Input.compass.enabled = true;

        StartCoroutine(CheckDirection());
    }

    IEnumerator CheckDirection()
    {
        while (isCheckingDirection)
        {
            yield return new WaitForSeconds(checkInterval);

            if (Permission.HasUserAuthorizedPermission(Permission.FineLocation))
            {
                using (var activity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity"))
                using (var wifiManager = activity.Call<AndroidJavaObject>("getSystemService", "wifi"))
                using (var wifiInfo = wifiManager.Call<AndroidJavaObject>("getConnectionInfo"))
                {
                    string bssid = wifiInfo.Call<string>("getBSSID");
                    int rssi = CalibrateRSSI(wifiInfo.Call<int>("getRssi"));
                    rssiReadings.Add(rssi);

                    if (rssiReadings.Count > NumReadings)
                    {
                        rssiReadings.RemoveAt(0);
                    }

                    int averagedRSSI = CalculateAveragedRSSI();

                    int smoothedRSSI = SmoothRSSI(averagedRSSI);

                    Debug.Log($"BSSID: {bssid}");
                    Debug.Log($"Calibrated RSSI: {smoothedRSSI}");

                    MACValue = bssid;
                    RSSIValue = smoothedRSSI.ToString();

                    int trueHeading = (int)Input.compass.trueHeading;

                    if (trueHeading >= 90 && trueHeading <= 135)
                    {
                        Debug.Log("User is going east/southeast");
                        direction = "east/southeast";

                        // Add this block to determine the user's distance from the router
                        if (rssi > -45)
                        {
                            Debug.Log("User is within 2 meters of the router");
                            distance = 2;
                        }
                        else if (rssi > -49)
                        {
                            Debug.Log("User is within 4 meters of the router");
                            distance = 4;
                        }
                        else if (rssi > -55)
                        {
                            Debug.Log("User is within 6 meters of the router");
                            distance = 6;
                        }
                        else if (rssi > -58)
                        {
                            Debug.Log("User is within 8 meters of the router");
                            distance = 8;
                        }
                        else if (rssi > -61)
                        {
                            Debug.Log("User is within 10 meters of the router");
                            distance = 10;
                        }
                        else if (rssi > -63)
                        {
                            Debug.Log("User is within 12 meters of the router");
                            distance = 12;
                        }
                        else if (rssi > -65)
                        {
                            Debug.Log("User is within 14 meters of the router");
                            distance = 14;
                        }
                        else if (rssi > -74)
                        {
                            Debug.Log("User is within 16 meters of the router");
                            distance = 16;
                        }
                        else if (rssi > -78)
                        {
                            Debug.Log("User is within 18 meters of the router");
                            distance = 18;
                        }
                        else if (rssi > -80)
                        {
                            Debug.Log("User is more than 20 meters away from the router");
                            distance = 22;
                        }
                    }

                    else if (trueHeading >= 225 && trueHeading <= 270)
                    {
                        Debug.Log("User is going west/southwest");
                        direction = "west/southwest";

                        if (rssi > -52)
                        {
                            Debug.Log("User is within 2 meters of the router");
                            distance = 2;
                        }
                        else if (rssi > -60)
                        {
                            Debug.Log("User is within 4 meters of the router");
                            distance = 4;
                        }
                        else if (rssi > -62)
                        {
                            Debug.Log("User is within 6 meters of the router");
                            distance = 6;
                        }
                        else if (rssi > -63)
                        {
                            Debug.Log("User is within 8 meters of the router");
                            distance = 8;
                        }
                        else if (rssi > -64)
                        {
                            Debug.Log("User is within 10 meters of the router");
                            distance = 10;
                        }
                        else if (rssi > -65)
                        {
                            Debug.Log("User is within 12 meters of the router");
                            distance = 12;
                        }
                        else if (rssi > -70)
                        {
                            Debug.Log("User is within 14 meters of the router");
                            distance = 14;
                        }
                        else if (rssi > -73)
                        {
                            Debug.Log("User is within 16 meters of the router");
                            distance = 16;
                        }
                        else if (rssi > -76)
                        {
                            Debug.Log("User is within 18 meters of the router");
                            distance = 18;
                        }
                        else if (rssi > -78)
                        {
                            Debug.Log("User is within 20 meters of the router");
                            distance = 20;
                        }
                        else if (rssi > -80)
                        {
                            Debug.Log("User is more than 22 meters away from the router");
                            distance = 22;
                        }
                    }

                    // To save the values
                    PlayerPrefs.SetInt("trueHeading", trueHeading);
                    PlayerPrefs.SetString("Direction", direction);
                    PlayerPrefs.SetInt("Distance", distance);
                    PlayerPrefs.Save();

                }
            }
            else
            {
                Debug.Log("Fine location permission is not granted.");
                // Handle the lack of permissin (e.g., request permission again)
            }

            yield return new WaitForSeconds(checkInterval);

        }
    }

    private int CalibrateRSSI(int rawRSSI)
    {
        int calibratedRSSI = rawRSSI - CalibrationFactor;
        return calibratedRSSI;
    }

    private int CalculateAveragedRSSI()
    {
        int sum = 0;

        for (int i = 0; i < rssiReadings.Count; i++)
        {
            sum += rssiReadings[i];
        }

        return rssiReadings.Count > 0 ? sum / rssiReadings.Count : 0;
    }

    private int SmoothRSSI(int rawRSSI)
    {
        rssiBuffer.Add(rawRSSI);
        if (rssiBuffer.Count > bufferSize)
        {
            rssiBuffer.RemoveAt(0);
        }
        int smoothedRSSI = 0;

        if (rssiBuffer.Count > 0)
        {
            int sum = 0;
            foreach (int value in rssiBuffer)
            {
                sum += value;
            }
            smoothedRSSI = sum / rssiBuffer.Count;
        }

        return smoothedRSSI;
    }
}