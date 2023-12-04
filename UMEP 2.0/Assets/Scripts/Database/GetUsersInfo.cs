using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetUsersInfo : MonoBehaviour
{
    public Text id_number;

    // Start is called before the first frame update
    void Start()
    {
        // Retrieve the id_number from PlayerPrefs
        string idNumber = PlayerPrefs.GetString("id_number", "");

        id_number.text = idNumber;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
