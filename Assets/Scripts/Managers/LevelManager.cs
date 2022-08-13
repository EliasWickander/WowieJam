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

    private int m_mistakes = 0;
    public int Mistakes => m_mistakes;
    public event Action OnMistakeAdded; 
    private void Awake()
    {
        Instance = this;

        m_mistakes = 0;
    }

    public void AddMistake()
    {
        m_mistakes++;

        OnMistakeAdded?.Invoke();
        if (m_mistakes >= 3)
        {
            //game over
        }
    }
}
