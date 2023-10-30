using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingManager : MonoBehaviour
{
    public Image firstImage; // Reference to the initial image in the splash scene.
    public Image secondImage; // Reference to the next image in the main scene.
    public Image thirdImage;
    public float fadeDuration = 1.0f; // Duration of the fade animation in seconds.

    public string sceneName;

    private void Start()
    {
        StartCoroutine(TransitionImages());
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
}
