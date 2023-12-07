using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.Android;
using TMPro;

public class TenthfloorSetNavigationTarget : MonoBehaviour
{
    [SerializeField]
    private Button Reroute;
    [SerializeField]
    private GameObject EastExit, WestExit;
    [SerializeField]
    private GameObject ARCamera;
    [SerializeField]
    private TMP_Text compass;

    private NavMeshPath path;
    private LineRenderer line;
    private GameObject targetExit;

    public GameObject twoMEast, fourMEast, sixMEast, eightMEast, tenMEast, twelveMEast, fourteenMEast, sixteenMEast, eighteenMEast, twentyMEast;
    public GameObject twoMWest, fourMWest, sixMWest, eightMWest, tenMWest, twelveMWest, fourteenMWest, sixteenMWest, eighteenMWest, twentyMWest;

    void Start()
    {
        // Request location permission
        if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
        {
            Permission.RequestUserPermission(Permission.FineLocation);
        }

        Input.compass.enabled = true;

        Invoke("DelayShit", 2f);

        path = new NavMeshPath();
        line = transform.GetComponent<LineRenderer>();

        // Calculate the shortest path using A* algorithm
        // Determine which exit is closer at the start
        targetExit = Vector3.Distance(transform.position, EastExit.transform.position) < Vector3.Distance(transform.position, WestExit.transform.position) ? EastExit : WestExit;

        // Add an event listener for the Reroute button
        Reroute.onClick.AddListener(RerouteNavigation);
    }

    void DelayShit()    
    {
        if (Permission.HasUserAuthorizedPermission(Permission.FineLocation))
        {
            float trueHeading = Input.compass.trueHeading;

            compass.text = "Angle: " + trueHeading.ToString();

            // Rotate the ARCamera based on the true heading
            ARCamera.transform.rotation = Quaternion.Euler(0, trueHeading, 0);

            // Make the floorplan always face 20 degrees north
            //floorPlan.transform.rotation = Quaternion.Euler(0, 20, 0);
        }
        else
        {
            Debug.Log("Fine location permission is not granted.");
        }
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
}