using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PathFinder : MonoBehaviour
{

    private const int MOVE_STRAIGHT_COST = 10;
    private const int MOVE_DIAGONAL_COST = 14;

    [SerializeField]
    private Vector3 originPosition;
    [SerializeField]
    private int height;
    [SerializeField]
    private int width;
    [SerializeField]
    private float cellSize;

    private Grid<PathNode> _grid;
    private List<PathNode> openList;
    private List<PathNode> closedList;

    // Start is called before the first frame update
    void Start()
    {
        _grid = new Grid<PathNode>(height, width, cellSize, originPosition, (Grid<PathNode> g, int x, int y) => new PathNode(g, x, y));
    }

    public List<Vector3> FindPath(int startX, int startY, int endX, int endY)
    {
        List<PathNode> path = FindPathNodes(startX, startY, endX, endY);

        if (path == null) return null;

        List<Vector3> vectorPath = new List<Vector3>();
        foreach (PathNode pathNode in path)
        {
            
            vectorPath.Add(new Vector3(pathNode.X, 0, pathNode.Y) * cellSize + Vector3.one * cellSize * .5F);
        }

        return vectorPath;
    }

    public Vector3 GetWorldPosition(Vector3 gridPosition)
    {
        return new Vector3(gridPosition.x + _grid.OriginPosition.x, 0, gridPosition.z + _grid.OriginPosition.z);
    }

    public List<PathNode> FindPathNodes(int startX, int startY, int endX, int endY)
    {

        var startNode = _grid.GetValue(startX, startY);
        var endNode = _grid.GetValue(endX, endY);

        openList = new List<PathNode>() { startNode };
        closedList = new List<PathNode>();

        for (int x = 0; x < _grid.Witdh; x++)
        {
            for (int y = 0; y < _grid.Height; y++)
            {
                var pathNode = _grid.GetValue(x, y);
                pathNode.gCost = int.MaxValue;
                pathNode.CalculateFCost();
                pathNode.cameFromNode = null;
            }
        }

        startNode.gCost = 0;
        startNode.hCost = CalculateDistanceCost(startNode, endNode);
        startNode.CalculateFCost();

        while (openList.Count > 0)
        {
            var lowestFCost = openList.Min(n => n.fCost);
            PathNode currentNode = openList.FirstOrDefault(node => node.fCost == lowestFCost);
            if (currentNode == endNode)
            {
                return CalculatePath(endNode);
            }

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            foreach (var neighbourNode in GetNeighbourList(currentNode))
            {
                if (closedList.Contains(neighbourNode)) continue;

                int tentativeGCost = currentNode.gCost + CalculateDistanceCost(currentNode, neighbourNode);
                if (tentativeGCost < neighbourNode.gCost)
                {
                    neighbourNode.cameFromNode = currentNode;
                    neighbourNode.gCost = tentativeGCost;
                    neighbourNode.hCost = CalculateDistanceCost(neighbourNode, endNode);
                    neighbourNode.CalculateFCost();

                    if (!openList.Contains(neighbourNode))
                    {
                        openList.Add(neighbourNode);
                    }

                }
            }
        }

        return null;

    }

    private List<PathNode> GetNeighbourList(PathNode currentNode)
    {
        List<PathNode> neighbourList = new List<PathNode>();

        if (currentNode.X - 1 >= 0)
        {
            neighbourList.Add(GetNode(currentNode.X - 1, currentNode.Y));
            if (currentNode.Y - 1 >= 0) neighbourList.Add(GetNode(currentNode.X - 1, currentNode.Y - 1));
            if (currentNode.Y + 1 < _grid.Height) neighbourList.Add(GetNode(currentNode.X - 1, currentNode.Y + 1));
        }
        if (currentNode.X + 1 < _grid.Witdh)
        {
            neighbourList.Add(GetNode(currentNode.X + 1, currentNode.Y));
            if (currentNode.Y - 1 >= 0) neighbourList.Add(GetNode(currentNode.X + 1, currentNode.Y - 1));
            if (currentNode.Y + 1 < _grid.Height) neighbourList.Add(GetNode(currentNode.X + 1, currentNode.Y + 1));
        }

        if (currentNode.Y - 1 >= 0) neighbourList.Add(GetNode(currentNode.X, currentNode.Y - 1));
        if (currentNode.Y + 1 < _grid.Height) neighbourList.Add(GetNode(currentNode.X, currentNode.Y + 1));

        return neighbourList;
    }


    private PathNode GetNode(int x, int y)
    {
        return _grid.GetValue(x, y);
    }


    private List<PathNode> CalculatePath(PathNode endNode)
    {
        List<PathNode> path = new List<PathNode>();
        path.Add(endNode);
        PathNode currentNode = endNode;
        while (currentNode.cameFromNode != null)
        {
            path.Add(currentNode.cameFromNode);
            currentNode = currentNode.cameFromNode;
        }
        path.Reverse();
        return path;
    }

    private int CalculateDistanceCost(PathNode a, PathNode b)
    {
        int xDistance = Mathf.Abs(a.X - b.X);
        int yDistance = Mathf.Abs(a.Y - b.Y);
        int remaining = Mathf.Abs(xDistance - yDistance);

        return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, yDistance) + MOVE_STRAIGHT_COST * remaining;

    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            var path = FindPath(0, 0, 0, width - 1);
        }
    }
}
