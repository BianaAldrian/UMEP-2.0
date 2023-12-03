using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectToActualCam : MonoBehaviour
{
    WebCamTexture webcam;
    public GameObject panel;

    // Start is called before the first frame update
    void Start()
    {
        webcam = new WebCamTexture();
        panel.GetComponent<Renderer>().material.mainTexture = webcam;
        webcam.Play();
    }
}
