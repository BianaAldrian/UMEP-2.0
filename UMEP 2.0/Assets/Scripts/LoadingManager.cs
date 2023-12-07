using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Android;

public class LoadingManager : MonoBehaviour
{
    public string IP;
    public Image firstImage; // Reference to the initial image in the splash scene.
    public Image secondImage; // Reference to the next image in the main scene.
    public Image thirdImage;
    public float fadeDuration = 1.0f; // Duration of the fade animation in seconds.

    public string sceneName;

    private void Start()
    {
        StartCoroutine(RequestPermissions());
    }

    private IEnumerator RequestPermissions()
    {
        // Request camera permission
        if (!Permission.HasUserAuthorizedPermission(Permission.Camera))
        {
            Permission.RequestUserPermission(Permission.Camera);
            yield return new WaitForSeconds(1); // Wait for the user to respond
        }

        // Request location permission
        if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
        {
            Permission.RequestUserPermission(Permission.FineLocation);
            yield return new WaitForSeconds(1); // Wait for the user to respond
        }

        // Check if permissions are granted
        if (Permission.HasUserAuthorizedPermission(Permission.Camera) &&
            Permission.HasUserAuthorizedPermission(Permission.FineLocation))
        {
            // Check if location service is enabled
            if (!Input.location.isEnabledByUser)
            {
                // Open location settings
                OpenLocationSettings();
            }
            else
            {
                StartCoroutine(TransitionImages());
            }
        }
        else
        {
            // If permissions are not granted, exit the app
            Application.Quit();
        }
    }

    private IEnumerator TransitionImages()
    {
        yield return new WaitForSeconds(1); // Optional delay before fading.

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
        SceneManager.LoadScene(sceneName); // Replace with your main scene's name.
    }

    void OpenLocationSettings()
    {
        #if UNITY_ANDROID
        using (var unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            var currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

            // Intent to open location settings
            var intent = new AndroidJavaObject("android.content.Intent", "android.settings.LOCATION_SOURCE_SETTINGS");

            // Start activity
            currentActivity.Call("startActivity", intent);
        }
        #endif
    }
}
