using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Building : MonoBehaviour
{
    public Sprite m_icon;

    public string m_houseNumber = "01";

    public TextMeshProUGUI m_houseNumberTextMesh;
    public GameObject m_highlightObject;

    public Transform m_entrancePoint;

    public NavNode ClosestNavNode { get; private set; }

    private void Awake()
    {
        m_houseNumberTextMesh.text = m_houseNumber;
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
