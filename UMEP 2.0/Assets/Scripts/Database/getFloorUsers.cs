using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class getFloorUsers : MonoBehaviour
{
    private string serverIP;
    public TMP_Dropdown dropdown;
    public TMP_Text listItemPrefab;
    public Transform listItemHolder;

    private void Start()
    {
        serverIP = PlayerPrefs.GetString("serverIP");

        // Subscribe to the OnValueChanged event to listen for changes in the dropdown
        dropdown.onValueChanged.AddListener(delegate
        {
            DropdownValueChanged(dropdown);
        });
    }

    // Method called when the dropdown value changes
    void DropdownValueChanged(TMP_Dropdown change)
    {

        // Get the chosen value using the value property
        int chosenValueIndex = change.value;

        int floor_num = 0;
        // Access the chosen value using the options list
        string floorNum = change.options[chosenValueIndex].text;

        Debug.Log(floorNum);

        if (floorNum == "Tenth Floor")
        {
            floor_num = 10;
        }
        if (floorNum == "Ninth Floor")
        {
            floor_num = 9;
        }

        StartCoroutine(GetFloorData(floor_num));
    }

    // Coroutine to send a request to the PHP script
    IEnumerator GetFloorData(int floor_num)
    {

        // Create form data
        WWWForm form = new WWWForm();
        form.AddField("floor_num", floor_num);

        // Send a POST request
        using (UnityWebRequest www = UnityWebRequest.Post($"http://{serverIP}/UMEP/register_user.php", form))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error: " + www.error);
            }
            else
            {
                // Parse the JSON response
                string json = www.downloadHandler.text;
                List<FloorCheckerData> floorCheckerDataList = JsonUtility.FromJson<List<FloorCheckerData>>(json);

                // Display the data in list items
                DisplayDataInListItems(floorCheckerDataList);
            }
        }
    }

    // Method to display the retrieved data in the list items
    void DisplayDataInListItems(List<FloorCheckerData> data)
    {
        // Clear existing list items
        foreach (Transform child in listItemHolder)
        {
            Destroy(child.gameObject);
        }

        // Instantiate new list items based on the size of the data
        foreach (var floorData in data)
        {
            TMP_Text listItem = Instantiate(listItemPrefab, listItemHolder);
            listItem.text = $"ID Number: {floorData.id_number}\nFloor Number: {floorData.floor_num}";
        }
    }

    // Define a data class to match the JSON structure
    [System.Serializable]
    public class FloorCheckerData
    {
        public string id_number;
        public int floor_num;
    }
}
