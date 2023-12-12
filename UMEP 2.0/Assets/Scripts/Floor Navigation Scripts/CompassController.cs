using UnityEngine;
using UnityEngine.Android;

public class CompassController : MonoBehaviour
{
    void Start()
    {
        // Request location permission
        if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
        {
            Permission.RequestUserPermission(Permission.FineLocation);
        }
        Input.compass.enabled = true;
    }

    void Update()
    {
        if (Permission.HasUserAuthorizedPermission(Permission.FineLocation))
        {
            int trueHeading = (int)Input.compass.trueHeading; // The current true heading

            Debug.Log("trueHeading: " + trueHeading);

            PlayerPrefs.SetInt("trueHeading", trueHeading);
            PlayerPrefs.Save();
        }
    }
}
