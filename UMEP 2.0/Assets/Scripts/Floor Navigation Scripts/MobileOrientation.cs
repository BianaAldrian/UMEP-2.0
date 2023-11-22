using UnityEngine;
using TMPro;

public class MobileOrientation : MonoBehaviour
{
    public TextMeshProUGUI angle, cardinal;

    void Start()
    {
        // Request location permission
        if (!UnityEngine.Android.Permission.HasUserAuthorizedPermission(UnityEngine.Android.Permission.FineLocation))
        {
            UnityEngine.Android.Permission.RequestUserPermission(UnityEngine.Android.Permission.FineLocation);
        }

        Input.compass.enabled = true;
    }

    void Update()
    {
        if (UnityEngine.Android.Permission.HasUserAuthorizedPermission(UnityEngine.Android.Permission.FineLocation))
        {
            float magneticHeading = Input.compass.magneticHeading;
            float trueHeading = Input.compass.trueHeading;
            float headingAccuracy = Input.compass.headingAccuracy;

            Debug.Log("Magnetic Heading: " + magneticHeading);
            Debug.Log("True Heading: " + trueHeading);
            Debug.Log("Heading Accuracy: " + headingAccuracy);

            // Calculate cardinal direction based on true heading
            string[] cardinals = { "N", "NE", "E", "SE", "S", "SW", "W", "NW" };
            int index = Mathf.RoundToInt(trueHeading / 45f) % 8;
            string cardinalDirection = cardinals[index];

            Debug.Log("Cardinal Direction: " + cardinalDirection);

            // Display the true heading and cardinal direction on the UI
            angle.text = "Angle: " + trueHeading.ToString();
            cardinal.text = "Cardinal: " + cardinalDirection;
        }
    }
}