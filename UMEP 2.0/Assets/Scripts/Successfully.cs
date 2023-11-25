using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Successfully : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        ShowRegistrationToast();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public ToastSuccessfully toastSuccessfully;

    void ShowRegistrationToast()
    {
        toastSuccessfully.ShowToast("Successfully Registered");
    }
}
