using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IP : MonoBehaviour
{
    public string IP_address;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        PlayerPrefs.SetString("IP", IP_address);
    }
}
