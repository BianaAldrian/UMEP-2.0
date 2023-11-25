using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using TMPro;

public class SetNavigationTarget : MonoBehaviour
{
    [SerializeField]
    private Button Reroute;
    [SerializeField]
    private GameObject EastExit, WestExit;

    private NavMeshPath path;
    private LineRenderer line;
    private GameObject targetExit;

    void Start()
    {
        path = new NavMeshPath();
        line = transform.GetComponent<LineRenderer>();

        // Determine which exit is closer at the start
        targetExit = Vector3.Distance(transform.position, EastExit.transform.position) < Vector3.Distance(transform.position, WestExit.transform.position) ? EastExit : WestExit;

        // Add an event listener for the Reroute button
        Reroute.onClick.AddListener(RerouteNavigation);

    }

    void Update()
    {
        // Calculate the shortest path using A* algorithm
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
    }
}