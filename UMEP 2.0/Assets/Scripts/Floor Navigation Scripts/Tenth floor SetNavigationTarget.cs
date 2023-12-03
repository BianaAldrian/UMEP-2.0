using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.Android;

public class TenthfloorSetNavigationTarget : MonoBehaviour
{
    [SerializeField]
    private Button Reroute;
    [SerializeField]
    private GameObject EastExit, WestExit;
    [SerializeField]
    private GameObject ARCamera, floorPlan;

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

        // Check if the compass is enabled, and enable it if not
        if (!Input.compass.enabled)
        {
            Input.compass.enabled = true;
        }

        // Rotate the camera based on the compass heading
        ARCamera.transform.rotation = Quaternion.Euler(0, Input.compass.trueHeading, 0);

        // Rotate the floorPlan in the opposite direction
        floorPlan.transform.rotation = Quaternion.Euler(0, -Input.compass.trueHeading, 0);

        Debug.Log(Input.compass.trueHeading);

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
}