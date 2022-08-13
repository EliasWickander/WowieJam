using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class DeliveryBotSpawner : MonoBehaviour
{
    [SerializeField] 
    private List<DeliveryBot> m_botPrefabs;
    
    [SerializeField] 
    private List<Transform> m_spawnPoints;

    [SerializeField] 
    private float m_correctAtStartPercentage = 0.8f;

    [SerializeField] 
    private float m_spawnDelayMin = 0.5f;
    
    [SerializeField] 
    private float m_spawnDelayMax = 2;

    [SerializeField] 
    private float m_startSpawnDelay = 1;

    private float m_spawnTimer = 0;

    private float m_currentSpawnDelay = 0;
    
    private BuildingManager m_buildingManager;

    public event Action<DeliveryBot> OnBotSpawned;

    private void Awake()
    {
        m_buildingManager = LevelManager.Instance.BuildingManager;

        m_currentSpawnDelay = m_startSpawnDelay;
    }

    private void Update()
    {
        if (m_spawnTimer < m_currentSpawnDelay)
        {
            m_spawnTimer += Time.deltaTime;
        }
        else
        {
            m_spawnTimer = 0;
            Spawn();
        }
    }

    private void Spawn()
    {
        Vector3 spawnPoint = m_spawnPoints[Random.Range(0, m_spawnPoints.Count)].position;
        DeliveryBot botToSpawn = m_botPrefabs[Random.Range(0, m_botPrefabs.Count)];
        
        DeliveryBot spawnedBot = Instantiate(botToSpawn);
        spawnedBot.transform.position = spawnPoint;

        spawnedBot.DesignatedTarget = GetRandomAvailableBuilding();
        
        if (Random.Range(0f, 1f) <= m_correctAtStartPercentage)
            spawnedBot.CurrentTarget = spawnedBot.DesignatedTarget;
        else
            spawnedBot.CurrentTarget = GetRandomAvailableBuilding(spawnedBot.DesignatedTarget);

        OnBotSpawned?.Invoke(spawnedBot);
        spawnedBot.Init();

        m_currentSpawnDelay = Random.Range(m_spawnDelayMin, m_spawnDelayMax);
    }
    
    private Building GetRandomAvailableBuilding(Building ignoredBuilding = null)
    {
        List<Building> availableBuildings = new List<Building>(m_buildingManager.Buildings);
        
        if (ignoredBuilding != null)
            availableBuildings.Remove(ignoredBuilding);

        int rand = Random.Range(0, availableBuildings.Count);

        return availableBuildings[rand];
    }
}
