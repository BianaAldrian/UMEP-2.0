using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class EventManager : MonoBehaviour
{
    public int maxDistance = 70;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ActivateEvent(int eventID)
    {
        if(eventID == 1) //University Of Makati Football Field
        {
            SceneManager.LoadScene("University Of Makati Football Field");
        }
    }
}
