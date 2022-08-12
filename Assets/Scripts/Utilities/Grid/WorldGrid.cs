using System;
using System.Collections;
using System.Collections.Generic;
using CustomUtils;
using UnityEditor;
using UnityEngine;

public abstract class WorldGrid<TNode> : MonoBehaviour where TNode : GridNode, new()
{
    [SerializeField] 
    private bool m_debug = true;

    [SerializeField] 
    private bool m_is3D = true;
    
    [SerializeField] 
    private Vector2 m_worldSize = new Vector2(10, 10);
    
    [SerializeField] 
    private float m_nodeRadius = 0.2f;

    private Grid<TNode> m_grid = null;
    public Grid<TNode> Grid => m_grid;

    protected virtual void Awake()
    {
        CreateGrid();
    }
    protected virtual void Start()
    {
        
    }

    private void CreateGrid()
    {
        m_grid = new Grid<TNode>(transform.position, m_worldSize, m_nodeRadius, m_is3D);
    }
    
    protected virtual void OnDrawGizmos()
    {
        if(!m_debug)
            return;
        
        if (!EditorApplication.isPlaying)
        {
            //Show preview of grid
            CreateGrid();   
            Gizmos.color = Color.cyan;
        }
        else
        {
            //Show grid that has been created
            Gizmos.color = Color.green;
        }

        var oldColor = Gizmos.color;
        for (int x = 0; x < m_grid.GridSize.x; x++)
        {
            for (int y = 0; y < m_grid.GridSize.y; y++)
            {
                GridNode node = m_grid.GetNode(x, y);

                Gizmos.DrawWireSphere(node.WorldPosition, m_grid.NodeRadius);

                Gizmos.color = oldColor;
            }
        }
        
        Gizmos.color = Color.white;
    }
}
