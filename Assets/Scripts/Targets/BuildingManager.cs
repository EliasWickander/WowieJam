using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class BuildingManager : MonoBehaviour
{
    [SerializeField]
    private List<Building> m_buildings = new List<Building>();
    public List<Building> Buildings => m_buildings;

    private void Awake()
    {
        m_buildings = FindObjectsOfType<Building>().ToList();
    }
}
