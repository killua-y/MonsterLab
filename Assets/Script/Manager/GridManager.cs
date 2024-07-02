using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridManager : Singleton<GridManager>
{
    public GameObject GridMap;

    public Graph graph;

    List<Tile> allTiles = new List<Tile>();

    protected override void Awake()
    {
        base.Awake();
        allTiles = GridMap.GetComponentsInChildren<Tile>().ToList();
        InitializeGraph();
    }

    private void InitializeGraph()
    {
        graph = new Graph();

        for (int i = 0; i < allTiles.Count; i++)
        {
            Vector2 place = allTiles[i].transform.position;
            graph.AddNode(place);
        }

        var allNodes = graph.Nodes;

        for (int i = 0; i < allNodes.Count; i++)
        {
            int rowIndex = i / 8; // There are 8 columns
            int columnIndex = i % 8; // Calculate the column index

            // Assign the rowIndex and columnIndex to the node
            allNodes[i].rowIndex = rowIndex;
            allNodes[i].columnIndex = columnIndex;

            if ((i >= 0) && (i <= 3))
            {
                allNodes[i].SetPlayerArea(true);
            }
            else if ((i >= 8) && (i <= 11))
            {
                allNodes[i].SetPlayerArea(true);
            }
            else if ((i >= 16) && (i <= 19))
            {
                allNodes[i].SetPlayerArea(true);
            }
            else if ((i >= 24) && (i <= 27))
            {
                allNodes[i].SetPlayerArea(true);
            }
            else if ((i >= 32) && (i <= 35))
            {
                allNodes[i].SetPlayerArea(true);
            }
        }

        foreach (Node from in allNodes)
        {
            foreach (Node to in allNodes)
            {
                if (Vector2.Distance(from.worldPosition, to.worldPosition) < 1.1f && from != to)
                {
                    graph.AddEdge(from, to);
                }
            }
        }
    }

    // 用BFS寻找距离给定的node最近的free node，同时需要满足对应team
    public Node GetFreeNode(int rowIndex, int columnIndex, bool forPlayerArea)
    {
        Node startNode = GetNodeForRowAndColumn(rowIndex, columnIndex);
        if (startNode == null)
        {
            return null;
        }

        // BFS setup
        Queue<Node> queue = new Queue<Node>();
        HashSet<Node> visited = new HashSet<Node>();

        queue.Enqueue(startNode);
        visited.Add(startNode);

        while (queue.Count > 0)
        {
            Node current = queue.Dequeue();

            if (!current.IsOccupied && current.IsPlayerArea == forPlayerArea)
            {
                return current;
            }

            // Add neighbors to the queue
            foreach (Node neighbor in graph.NodeNear(current))
            {
                if (!visited.Contains(neighbor))
                {
                    queue.Enqueue(neighbor);
                    visited.Add(neighbor);
                }
            }
        }

        return null; // No free node found that matches the criteria
    }


    public List<Node> GetPath(Node from, Node to)
    {
        return graph.PathSearch(from, to);
    }

    public List<Node> GetNodesCloseTo(Node to)
    {
        return graph.Neighbors(to);
    }

    public Node GetNodeForTile(Tile t)
    {
        var allNodes = graph.Nodes;

        for (int i = 0; i < allNodes.Count; i++)
        {
            if (t.transform.GetSiblingIndex() == allNodes[i].index)
            {
                return allNodes[i];
            }
        }

        return null;
    }

    public Node GetNodeForIndex(int _index)
    {
        var allNodes = graph.Nodes;

        for (int i = 0; i < allNodes.Count; i++)
        {
            if (_index == allNodes[i].index)
            {
                return allNodes[i];
            }
        }

        return null;
    }

    public Node GetNodeForRowAndColumn(int row, int column)
    {
        var allNodes = graph.Nodes;

        for (int i = 0; i < allNodes.Count; i++)
        {
            if ((row == allNodes[i].rowIndex) && (column == allNodes[i].columnIndex))
            {
                return allNodes[i];
            }
        }

        return null;
    }

    public Tile GetTileForRowAndColumn(int row, int column)
    {
        int index = row * 8 + column;

        return allTiles[index];
    }

    public int fromIndex = 0;
    public int toIndex = 0;

    private void OnDrawGizmos()
    {
        if (graph == null)
            return;

        var allEdges = graph.Edges;
        if (allEdges == null)
            return;

        foreach (Edge e in allEdges)
        {
            //Debug.DrawLine(e.from.worldPosition, e.to.worldPosition, Color.black, 100);
        }

        var allNodes = graph.Nodes;
        if (allNodes == null)
            return;

        foreach (Node n in allNodes)
        {
            Gizmos.color = n.IsOccupied ? Color.red : Color.green;
            Gizmos.DrawSphere(n.worldPosition, 0.1f);

        }

        if (fromIndex >= allNodes.Count || toIndex >= allNodes.Count)
            return;

        List<Node> path = graph.PathSearch(allNodes[fromIndex], allNodes[toIndex]);
        if (path.Count > 1)
        {
            for (int i = 1; i < path.Count; i++)
            {
                Debug.DrawLine(path[i - 1].worldPosition, path[i].worldPosition, Color.red, 1);
            }
        }
    }
}