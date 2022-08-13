using System.Collections;
using System.Collections.Generic;
using CustomUtils;
using UnityEditor;
using UnityEngine;

public class NavNode : GridNode
{
    public bool m_walkable;
}

public class NavGrid : WorldGrid<NavNode>
{
    public LayerMask m_walkableMask;
    protected override void Awake()
    {
        base.Awake();

        Grid.OnNodeCreated += OnNodeCreated;
        Grid.CreateGrid();
    }

    private void OnNodeCreated(GridNode node)
    {
        NavNode navNode = node as NavNode;

        navNode.m_walkable = Physics.CheckSphere(node.WorldPosition, 0.5f, m_walkableMask);
    }

    public NavNode GetClosestWalkableNode(Vector3 position)
    {
        float closestDist = Mathf.Infinity;
        NavNode closestNode = Grid.GetNode(position);

        foreach (NavNode node in Grid.Nodes)
        {
            Vector3 dirToNode = node.WorldPosition - position;

            if (dirToNode.sqrMagnitude < closestDist && node.m_walkable)
            {
                closestDist = dirToNode.sqrMagnitude;
                closestNode = node;
            }
        }

        return closestNode;
    }
    
    protected override void OnDrawGizmos()
    {
        if(!m_debug)
            return;
        
        if (!EditorApplication.isPlaying)
        {
            //Show preview of grid
            Awake();  
            Gizmos.color = Color.cyan;
        }
        else
        {
            //Show grid that has been created
            Gizmos.color = Color.green;
        }

        var oldColor = Gizmos.color;
        for (int x = 0; x < Grid.GridSize.x; x++)
        {
            for (int y = 0; y < Grid.GridSize.y; y++)
            {
                NavNode node = Grid.GetNode(x, y);

                if(!node.m_walkable)
                    Gizmos.color = Color.red;
                
                Gizmos.DrawWireSphere(node.WorldPosition, Grid.NodeRadius);

                Gizmos.color = oldColor;
            }
        }
        
        Gizmos.color = Color.white;
    }
}
