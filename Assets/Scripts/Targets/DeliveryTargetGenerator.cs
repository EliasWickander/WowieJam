using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class TargetData
{
    public GameObject m_targetPrefab;
}

public class DeliveryTargetGenerator : MonoBehaviour
{
    [SerializeField] 
    private List<TargetData> m_targetTypes;

    [SerializeField] 
    private List<Transform> m_spawnPoints;

    [SerializeField] 
    private int m_amountToSpawn;

    [SerializeField] 
    private NavGrid m_navGrid;

    private List<Transform> m_availableSpawnPoints = null;

    private List<GameObject> m_generatedTargets = new List<GameObject>();
    public List<GameObject> GeneratedTargets => m_generatedTargets;
    
    private void Awake()
    {
        Generate();
    }

    private void Generate()
    {
        m_availableSpawnPoints = new List<Transform>(m_spawnPoints);

        for (int i = 0; i < m_amountToSpawn; i++)
        {
            if (m_availableSpawnPoints.Count <= 0)
            {
                Debug.LogError("Attempted to generate target but all spawn points are occupied");
                return;
            }
            
            TargetData targetData = m_targetTypes[Random.Range(0, m_targetTypes.Count)];
            Transform spawnPoint = m_availableSpawnPoints[Random.Range(0, m_availableSpawnPoints.Count)];

            Vector3 spawnPos = m_navGrid.Grid.GetNode(spawnPoint.position).WorldPosition;

            GameObject newTarget = Instantiate(targetData.m_targetPrefab);
            newTarget.transform.position = spawnPos;
        
            m_availableSpawnPoints.Remove(spawnPoint);   
            m_generatedTargets.Add(newTarget);
        }
    }
}
