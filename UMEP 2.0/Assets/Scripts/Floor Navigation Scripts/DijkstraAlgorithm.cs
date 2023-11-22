using System;
using System.Collections.Generic;
using UnityEngine;

public class DijkstraAlgorithm : MonoBehaviour
{
    // Class to represent a graph node
    public class Node
    {
        public string name;
        public List<Edge> edges;

        public Node(string nodeName)
        {
            name = nodeName;
            edges = new List<Edge>();
        }
    }

    // Class to represent an edge between two nodes
    public class Edge
    {
        public Node destination;
        public int weight;

        public Edge(Node dest, int w)
        {
            destination = dest;
            weight = w;
        }
    }

    // Dijkstra's algorithm function
    public List<Node> Dijkstra(Node start, Node end)
    {
        Dictionary<Node, int> distances = new Dictionary<Node, int>();
        Dictionary<Node, Node> previousNodes = new Dictionary<Node, Node>();
        List<Node> unvisitedNodes = new List<Node>();

        foreach (var node in GetComponentsInChildren<Node>())
        {
            distances[node] = int.MaxValue;
            previousNodes[node] = null;
            unvisitedNodes.Add(node);
        }

        distances[start] = 0;

        while (unvisitedNodes.Count > 0)
        {
            Node current = null;
            foreach (var node in unvisitedNodes)
            {
                if (current == null || distances[node] < distances[current])
                {
                    current = node;
                }
            }

            unvisitedNodes.Remove(current);

            foreach (var neighborEdge in current.edges)
            {
                int alt = distances[current] + neighborEdge.weight;
                if (alt < distances[neighborEdge.destination])
                {
                    distances[neighborEdge.destination] = alt;
                    previousNodes[neighborEdge.destination] = current;
                }
            }
        }

        // Reconstruct the path
        List<Node> path = new List<Node>();
        Node currentNode = end;
        while (currentNode != null)
        {
            path.Insert(0, currentNode);
            currentNode = previousNodes[currentNode];
        }

        return path;
    }

    // Example usage
    private void Start()
    {
        // Create nodes
        Node nodeA = new Node("A");
        Node nodeB = new Node("B");
        Node nodeC = new Node("C");
        Node nodeD = new Node("D");

        // Create edges
        nodeA.edges.Add(new Edge(nodeB, 2));
        nodeB.edges.Add(new Edge(nodeA, 2));
        nodeB.edges.Add(new Edge(nodeC, 1));
        nodeC.edges.Add(new Edge(nodeB, 1));
        nodeC.edges.Add(new Edge(nodeD, 3));
        nodeD.edges.Add(new Edge(nodeC, 3));

        // Find the shortest path from node A to node D
        List<Node> shortestPath = Dijkstra(nodeA, nodeD);

        // Output the result
        foreach (var node in shortestPath)
        {
            Debug.Log(node.name);
        }
    }
}
