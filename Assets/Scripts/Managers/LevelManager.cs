using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    [SerializeField]
    private BuildingManager m_buildingManager;

    public BuildingManager BuildingManager => m_buildingManager;
    
    private void Awake()
    {
        Instance = this;
    }
}
