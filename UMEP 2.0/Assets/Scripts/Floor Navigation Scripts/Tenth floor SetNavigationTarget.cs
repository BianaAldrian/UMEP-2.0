using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.Android;

public class TenthfloorSetNavigationTarget : MonoBehaviour
{
    [SerializeField]
    private Button Reroute;
    [SerializeField]
    private GameObject EastExit, WestExit;
    [SerializeField]
    private GameObject ARCamera;

    private NavMeshPath path;
    private LineRenderer line;
    private GameObject targetExit;

    public GameObject twoMEast, fourMEast, sixMEast, eightMEast, tenMEast, twelveMEast, fourteenMEast, sixteenMEast, eighteenMEast, twentyMEast;
    public GameObject twoMWest, fourMWest, sixMWest, eightMWest, tenMWest, twelveMWest, fourteenMWest, sixteenMWest, eighteenMWest, twentyMWest;

    private bool gyroEnabled;
    private Gyroscope gyro;
    private Quaternion initialRotation;


    void Start()
    {
        // Request location permission
        if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
        {
            Permission.RequestUserPermission(Permission.FineLocation);
        }

        gyroEnabled = EnableGyro();

        if (!gyroEnabled)
        {
            Debug.LogError("Gyroscope not supported on this device.");
        }
        else
        {
            Input.compass.enabled = true; // Enable compass
            initialRotation = Quaternion.Euler(0, Input.compass.trueHeading, 0);

            ARCamera.transform.rotation = Quaternion.Euler(0, Input.compass.trueHeading, 0);
        }

        //RotateARCamera();

        // To load the values in another scene
        //float trueHeading = PlayerPrefs.GetFloat("trueHeading");
        //string direction = PlayerPrefs.GetString("Direction");
        //int distance = PlayerPrefs.GetInt("Distance");

        //float trueHeading = Input.compass.trueHeading;
        //ARCamera.transform.Rotate(0, trueHeading, 0);

        //ARCamera.transform.rotation = Quaternion.Euler(0f, trueHeading, 0f);

        //ARCamera.transform.position = twoMEast.transform.position;

        /*GameObject targetObject = null;

        if (direction == "east/southeast")
        {
            switch (distance)
            {
                case 2:
                    targetObject = twoMEast;
                    break;
                case 4:
                    targetObject = fourMEast;
                    break;
                case 6:
                    targetObject = sixMEast;
                    break;
                case 8:
                    targetObject = eightMEast;
                    break;
                case 10:
                    targetObject = tenMEast;
                    break;
                case 12:
                    targetObject = twelveMEast;
                    break;
                case 14:
                    targetObject = fourteenMEast;
                    break;
                case 16:
                    targetObject = sixteenMEast;
                    break;
                case 18:
                    targetObject = eighteenMEast;
                    break;
                case 20:
                    targetObject = twentyMEast;
                    break;
                default:
                    Debug.Log("Invalid distance");
                    break;
            }
        }
        else if (direction == "west/southwest")
        {
            switch (distance)
            {
                case 2:
                    targetObject = twoMWest;
                    break;
                case 4:
                    targetObject = fourMWest;
                    break;
                case 6:
                    targetObject = sixMWest;
                    break;
                case 8:
                    targetObject = eightMWest;
                    break;
                case 10:
                    targetObject = tenMWest;
                    break;
                case 12:
                    targetObject = twelveMWest;
                    break;
                case 14:
                    targetObject = fourteenMWest;
                    break;
                case 16:
                    targetObject = sixteenMWest;
                    break;
                case 18:
                    targetObject = eighteenMWest;
                    break;
                case 20:
                    targetObject = twentyMWest;
                    break;
                default:
                    Debug.Log("Invalid distance");
                    break;
            }
        }
        else
        {
            Debug.Log("Invalid direction");
        }
        if (targetObject == null)
        {
            Debug.Log("Invalid direction or distance");
        }

        if (targetObject != null)
        {
            ARCamera.transform.position = targetObject.transform.position;
        }
        else
        {
            Debug.Log("Invalid direction or distance");
        }*/

        path = new NavMeshPath();
        line = transform.GetComponent<LineRenderer>();

        // Calculate the shortest path using A* algorithm
        // Determine which exit is closer at the start
        targetExit = Vector3.Distance(transform.position, EastExit.transform.position) < Vector3.Distance(transform.position, WestExit.transform.position) ? EastExit : WestExit;

        // Add an event listener for the Reroute button
        Reroute.onClick.AddListener(RerouteNavigation);
    }

    void Update()
    {
        NavMesh.CalculatePath(transform.position, targetExit.transform.position, NavMesh.AllAreas, path);

        // Set the line renderer positions to the corners of the calculated path
        line.positionCount = path.corners.Length;
        line.SetPositions(path.corners);

        // Enable the line renderer so it is visible
        line.enabled = true;
    }

    // Method to be called when the Reroute button is clicked
    private void RerouteNavigation()
    {
        // Change the target exit when reroute is clicked
        targetExit = targetExit == EastExit ? WestExit : EastExit;

        // Recalculate the path
        NavMesh.CalculatePath(transform.position, targetExit.transform.position, NavMesh.AllAreas, path);
    }

    bool EnableGyro()
    {
        if (SystemInfo.supportsGyroscope)
        {
            gyro = Input.gyro;
            gyro.enabled = true;
            return true;
        }

        return false;
    }

}