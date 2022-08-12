using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode
{
    public Vector3 WorldPosition { get; set; }
        
    public bool Walkable { get; set; }
    //Distance from this node to start node
    public int GCost { get; set; } = int.MaxValue;
    
    //Distance from this node to target node
    public int HCost { get; set; }
    public int FCost => GCost + HCost;

    public PathNode PrevNode { get; set; } = null;
}

public class Pathfinding : MonoBehaviour
{
    private const int MOVE_STRAIGHT_COST = 10;
    private const int MOVE_DIAGONAL_COST = 14;
    
    [SerializeField] 
    private NavGrid m_grid;
    
    //Find closest path between two points using A* pathfinding
    public List<PathNode> FindPath(Vector3 startPoint, Vector3 endPoint)
    {
        PathNode[,] nodes = new PathNode[m_grid.Grid.GridSize.x, m_grid.Grid.GridSize.y];

        for (int x = 0; x < m_grid.Grid.GridSize.x; x++)
        {
            for (int y = 0; y < m_grid.Grid.GridSize.y; y++)
            {
                nodes[x, y] = new PathNode();
                nodes[x, y].WorldPosition = m_grid.Grid.Nodes[x, y].WorldPosition;
                nodes[x, y].Walkable = m_grid.Grid.Nodes[x, y].m_walkable;
            }
        }
        
        NavNode startNode = m_grid.Grid.GetNode(startPoint);
        PathNode startPfNode = nodes[startNode.GridPos.x, startNode.GridPos.y];

        if (!startNode.m_walkable)
        {
            Debug.LogError("Pathfinding failed: Start node is obstacle");
            return null;
        }
        
        NavNode endNode = m_grid.Grid.GetNode(endPoint);
        PathNode endPfNode = nodes[endNode.GridPos.x, endNode.GridPos.y];
        
        List<PathNode> openList = new List<PathNode>() {startPfNode};
        List<PathNode> closedList = new List<PathNode>();

        startPfNode.GCost = 0;
        startPfNode.HCost = CalculateDistanceCost(startNode, endNode);
        
        while (openList.Count > 0)
        {
            //Find most attractive node to investigate
            PathNode currentPfNode = GetLowestFCostNode(openList);
            NavNode currentNavNode = m_grid.Grid.GetNode(currentPfNode.WorldPosition);

            //If at end node, we found a path
            if (currentPfNode == endPfNode)
            {
                return CalculatePath(endPfNode);
            }
            
            openList.Remove(currentPfNode);
            closedList.Add(currentPfNode);

            //Check all the neighbors of current node
            foreach (NavNode neighbour in m_grid.Grid.GetNeighbours(currentNavNode))
            {
                PathNode neighbourPfNode = nodes[neighbour.GridPos.x, neighbour.GridPos.y];
                
                if (!neighbourPfNode.Walkable || closedList.Contains(neighbourPfNode))
                    continue;

                //If neighbour is closer to goal than current node 
                int tentativeGCost = currentPfNode.GCost + CalculateDistanceCost(currentNavNode, neighbour);
                if (tentativeGCost < neighbourPfNode.GCost)
                {
                    neighbourPfNode.PrevNode = currentPfNode;
                    neighbourPfNode.GCost = tentativeGCost;
                    neighbourPfNode.HCost = CalculateDistanceCost(neighbour, endNode);

                    if (!openList.Contains(neighbourPfNode))
                    {
                        openList.Add(neighbourPfNode);
                    }
                }
            }
        }

        Debug.LogError("Pathfinding failed: Couldn't find path to end point");
        return null;
    }

    //Retrace the path from node
    private List<PathNode> CalculatePath(PathNode endNode)
    {
        List<PathNode> path = new List<PathNode>() {endNode};

        PathNode currentNode = endNode;

        while (currentNode.PrevNode != null)
        {
            path.Add(currentNode.PrevNode);
            currentNode = currentNode.PrevNode;
        }
        
        path.Reverse();
        return path;
    }

    //Get node in list with lowest F cost
    private PathNode GetLowestFCostNode(List<PathNode> nodeList)
    {
        PathNode lowestNode = nodeList[0];
        
        foreach (PathNode node in nodeList)
        {
            if (node.FCost < lowestNode.FCost)
            {
                lowestNode = node;
            }
        }

        return lowestNode;
    }
    
    //Calculate distance cost between two nodes
    private int CalculateDistanceCost(NavNode first, NavNode second)
    {
        int xDist = Mathf.Abs(second.GridPos.x - first.GridPos.x);
        int yDist = Mathf.Abs(second.GridPos.y - first.GridPos.y);
        int dist = Mathf.Abs(xDist - yDist);

        return MOVE_DIAGONAL_COST * Mathf.Min(xDist, yDist) + MOVE_STRAIGHT_COST * dist;
    }
}