using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ToastSuccessfully : MonoBehaviour
{
    public Text toastText;
    public float toastDuration = 2.0f;

    public static ToastSuccessfully instance; // Singleton pattern

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    public void ShowToast(string message)
    {
        StartCoroutine(ShowToastCoroutine(message));
    }

    private IEnumerator ShowToastCoroutine(string message)
    {
        toastText.text = message;
        toastText.enabled = true;

        yield return new WaitForSeconds(toastDuration);

        toastText.enabled = false;
    }

    //use this code to call the toast in your script
    /*public ToastManager toastManager;

    void ShowRegistrationToast()
    {
        toastManager.ShowToast("Successfully Registered");
    }*/

}
