using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using System.Collections;

public class Login : MonoBehaviour
{
    public string next_scene, user_type;
    public TMP_Text ID_error, password_error;
    public TMP_InputField id_number, password;
    public Button proceed;

    // Start is called before the first frame update
    void Start()
    {
        // Attach the CheckInputs method to the onClick event of the submit button
        proceed.onClick.AddListener(CheckInputs);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Method to check inputs when the submit button is clicked
    void CheckInputs()
    {
        // Check if ID Number is empty
        if (string.IsNullOrEmpty(id_number.text))
        {
            // Display error message in the input field with error color
            id_number.placeholder.GetComponent<TextMeshProUGUI>().color = Color.red;
            id_number.placeholder.GetComponent<TextMeshProUGUI>().text = "ID Number cannot be empty!";
            return; // Stop further processing as ID Number is required
        }

        // Check if Password is empty
        if (string.IsNullOrEmpty(password.text))
        {
            // Display error message in the input field with error color
            password.placeholder.GetComponent<TextMeshProUGUI>().color = Color.red;
            password.placeholder.GetComponent<TextMeshProUGUI>().text = "Password cannot be empty!";
            return; // Stop further processing as Password is required
        }

        // Both fields are filled, call the LoginUser method
        //LoginUser();
    }

    /*
    void LoginUser()
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
            StartCoroutine(LoginUserCoroutine(serverIP));
        }
    }
    */

    IEnumerator LoginUserCoroutine(string serverIP)
    {
        WWWForm form = new WWWForm();
        form.AddField("id_number", id_number.text);
        form.AddField("password", password.text);
        form.AddField("user_type", user_type);

        UnityWebRequest webRequest = UnityWebRequest.Post($"http://{serverIP}/UMEP/login_user.php", form);
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

            if (response == "!exist")
            {
                // Display error message in the input field with error color
                id_number.placeholder.GetComponent<TextMeshProUGUI>().color = Color.red;
                id_number.placeholder.GetComponent<TextMeshProUGUI>().text = "User does not exist!";
            }
            else if (response == "invalid_user")
            {
                // Display error message in the input field with error color
                id_number.placeholder.GetComponent<TextMeshProUGUI>().color = Color.red;
                id_number.placeholder.GetComponent<TextMeshProUGUI>().text = "Invalid user";
            }
            else if (response == "invalid_password")
            {
                // Display error message in the input field with error color
                password.placeholder.GetComponent<TextMeshProUGUI>().color = Color.red;
                password.placeholder.GetComponent<TextMeshProUGUI>().text = "Invalid password!";
            }
            else if (response == "valid_password")
            {
                // Password is valid, you can proceed with your logic here
                Debug.Log("Login successful!");
                PlayerPrefs.SetString("id_number", id_number.text);
                GoToNextScene();
            }
        }
    }

    void GoToNextScene()
    {
        // Use SceneManager to load the next scene
        SceneManager.LoadScene(next_scene);
    }
}
