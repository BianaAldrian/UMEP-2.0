using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Logout : MonoBehaviour
{
    public Button LogoutBtn;

    // Start is called before the first frame update
    void Start()
    {
        LogoutBtn.onClick.AddListener(logOut);
    }

    // Update is called once per frame
    void logOut()
    {
        PlayerPrefs.DeleteKey("id_number");
    }
}
