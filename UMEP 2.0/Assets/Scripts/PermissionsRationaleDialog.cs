using UnityEngine;

public class PermissionsRationaleDialog : MonoBehaviour
{
    const string permission = "android.permission.READ_PHONE_STATE";

    void Start()
    {
        if (!UnityEngine.Android.Permission.HasUserAuthorizedPermission(permission))
        {
            UnityEngine.Android.Permission.RequestUserPermission(permission);
        }
    }

    void OnGUI()
    {
        if (!UnityEngine.Android.Permission.HasUserAuthorizedPermission(permission))
        {
            // The user denied permission to use the microphone.
            // Display a message explaining why you need it with Yes/No buttons.
            // If the user says yes then present the request again
            // Display a dialog here.
            GUI.Label(new Rect(10, 10, 500, 60), "Would you like to allow access to the Phone State?");
            if (GUI.Button(new Rect(10, 80, 50, 30), "Yes"))
            {
                UnityEngine.Android.Permission.RequestUserPermission(permission);
            }
            if (GUI.Button(new Rect(70, 80, 50, 30), "No"))
            {
                // user declined, do nothing
            }
        }
        else
        {
            // The user authorized use of the microphone, update the status.
            GUI.Label(new Rect(10, 10, 500, 60), "Status: User authorized access to the phone state");
        }
    }
}