using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Building : MonoBehaviour
{
    public Sprite m_icon;

    public GameObject m_highlightObject;

    public Transform m_entrancePoint;

    public NavNode ClosestNavNode { get; private set; }

    private void Awake()
    {

    }

    private void Start()
    {
        ClosestNavNode = LevelManager.Instance.NavGrid.GetClosestWalkableNode(m_entrancePoint.position);
    }

    public void SetHighlighted(bool enabled) 
    {
        m_highlightObject.SetActive(enabled);
    }
}
