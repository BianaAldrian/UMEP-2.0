using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using System.Collections;

public class Register : MonoBehaviour
{
    public CheckConnection checkConnection; //Calling other script to connect

    public string next_scene, user_type;
    public TMP_Text errorHolder;
    public TMP_InputField id_number, mobile_number, password, password2;
    public Button submit;

    // Start is called before the first frame update
    void Start()
    {
        // Attach the CheckInputs method to the onClick event of the submit button
        submit.onClick.AddListener(CheckInputs);
    }

    // Method to check inputs when the submit button is clicked
    void CheckInputs()
    {
        // Reset error messages
        errorHolder.text = "";

        // Check if either id_number or mobile_number is empty
        if (string.IsNullOrEmpty(id_number.text) || string.IsNullOrEmpty(mobile_number.text))
        {
            // Display error message in the input field with error color
            if (string.IsNullOrEmpty(id_number.text))
            {
                id_number.placeholder.GetComponent<TextMeshProUGUI>().color = Color.red;
                id_number.placeholder.GetComponent<TextMeshProUGUI>().text = "ID Number cannot be empty!";
            }

            if (string.IsNullOrEmpty(mobile_number.text))
            {
                mobile_number.placeholder.GetComponent<TextMeshProUGUI>().color = Color.red;
                mobile_number.placeholder.GetComponent<TextMeshProUGUI>().text = "Mobile Number cannot be empty!";
            }

            // Display a general error message
            errorHolder.text = "Please fill in all required fields.";
        }
        else if (password.text != password2.text)
        {
            // Passwords do not match
            password.placeholder.GetComponent<TextMeshProUGUI>().color = Color.red;
            password.placeholder.GetComponent<TextMeshProUGUI>().text = "Passwords do not match!";

            password2.placeholder.GetComponent<TextMeshProUGUI>().color = Color.red;
            password2.placeholder.GetComponent<TextMeshProUGUI>().text = "Passwords do not match!";

            // Display a general error message
            errorHolder.text = "Passwords do not match.";
        }
        else
        {
            // Inputs are not empty and passwords match, you can proceed with your registration logic here
            CheckUser();
        }
    }

    void CheckUser()
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
            StartCoroutine(RegisterUserCoroutine(serverIP));
        }
    }

    IEnumerator CheckUserCoroutine(string serverIP)
    {
        WWWForm form = new WWWForm();
        form.AddField("id_number", id_number.text);

        UnityWebRequest webRequest = UnityWebRequest.Post($"http://{serverIP}/UMEP/check_user.php", form);
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

            if (response == "exist")
            {
                errorHolder.text = "ID already exists! Please try logging in.";
            }
            else
            {
                StartCoroutine(RegisterUserCoroutine(serverIP));
            }
        }
    }

    IEnumerator RegisterUserCoroutine(string serverIP)
    {
        WWWForm form = new WWWForm();
        form.AddField("id_number", id_number.text);
        form.AddField("mobile_number", mobile_number.text);
        form.AddField("password", password.text);
        form.AddField("user_type", user_type);

        UnityWebRequest webRequest = UnityWebRequest.Post($"http://{serverIP}/UMEP/register_user.php", form);
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

            if (response == "exist")
            {
                errorHolder.text = "ID already exists! Please try logging in.";
            }

            if (response == "successful")
            {
                // Registration is successful, go to the next scene
                GoToNextScene();
            }
        }
    }

    void GoToNextScene()
    {
        // Use SceneManager to load the next scene
        SceneManager.LoadScene(next_scene);
    }

    // Update is called once per frame
    void Update()
    {
        // You can add any additional update logic here if needed
    }
}