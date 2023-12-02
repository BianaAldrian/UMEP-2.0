using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConnectToActualCamera : MonoBehaviour
{
    WebCamTexture webcam;
    public RawImage img;

    // Start is called before the first frame update
    void Start()
    {
        webcam = new WebCamTexture();
        img.texture = webcam;
        webcam.Play();
    }
}
