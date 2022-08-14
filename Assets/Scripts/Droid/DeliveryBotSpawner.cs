using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

[Serializable]
public class BotSpawnData
{
    public DeliveryBot m_botPrefab;
    public int m_amount = 5;
    public Dictionary<string, bool> m_botsInAction = new Dictionary<string, bool>();
}

public class DeliveryBotSpawner : MonoBehaviour
{
    [SerializeField] 
    private List<Transform> m_spawnPoints;

    [SerializeField] 
    public float m_correctAtStartPercentage = 0.8f;

    [SerializeField] 
    public float m_malfunctionPercentage = 0.8f;
    
    private float m_spawnTimer = 0;

    private float m_currentSpawnDelay = 0;
    
    private BuildingManager m_buildingManager;

    public event Action<DeliveryBot> OnBotSpawned;

    public event Action<DeliveryBot> OnBotDelivered;
    
    private int m_spawnCount = 0;
    private LevelData m_currentLevel = null;

    private int m_botsAlive = 0;
    public bool m_canSpawn = true;

    public event Action OnAllBotsSpawnedAndDestroyed;

    private void Awake()
    {
        m_buildingManager = LevelManager.Instance.BuildingManager;
    }

    private void OnEnable()
    {
        LevelManager.Instance.OnLevelStart += OnLevelStart;
    }

    private void OnDisable()
    {
        LevelManager.Instance.OnLevelStart -= OnLevelStart;
    }

    private void OnLevelStart(LevelData level)
    {
        foreach (BotSpawnData spawnData in level.m_availableBotsData)
        {
            for (int i = 0; i < spawnData.m_amount; i++)
            {
                spawnData.m_botsInAction.Add(spawnData.m_botPrefab.Prefix + (i + 1), false);
            }
        }
        
        m_currentLevel = level;

        m_currentSpawnDelay = Random.Range(level.m_spawnDelayMin, level.m_spawnDelayMax);

        m_botsAlive = 0;
        m_spawnCount = 0;
    }

    private void OnBotFinishedDelivery(DeliveryBot bot)
    {
        OnBotDelivered?.Invoke(bot);
    }

    private void Update()
    {
        if (m_currentLevel != null)
        {
            if (m_canSpawn && m_spawnCount < m_currentLevel.m_amountDroids)
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
        }
    }

    private void Spawn()
    {
        List<BotSpawnData> availableSpawnData = new List<BotSpawnData>();

        foreach (BotSpawnData botSpawnData in m_currentLevel.m_availableBotsData)
        {
            for (int i = 0; i < botSpawnData.m_amount; i++)
            {
                if (!botSpawnData.m_botsInAction[botSpawnData.m_botPrefab.Prefix + (i + 1)])
                {
                    availableSpawnData.Add(botSpawnData);
                    break;
                }
            }
        }
        
        if (availableSpawnData.Count <= 0)
        {
            //All bots are in action. No more bots to spawn
            return;
        }
        BotSpawnData spawnData = availableSpawnData[Random.Range(0, availableSpawnData.Count)];
        
        Vector3 spawnPoint = m_spawnPoints[Random.Range(0, m_spawnPoints.Count)].position;
        DeliveryBot botToSpawn = spawnData.m_botPrefab;
        
        DeliveryBot spawnedBot = Instantiate(botToSpawn);
        spawnedBot.transform.position = spawnPoint;

        spawnedBot.DesignatedTarget = GetRandomAvailableBuilding();

        spawnedBot.spawnData = spawnData;
        if (Random.Range(0f, 1f) < m_correctAtStartPercentage)
            spawnedBot.CurrentTarget = spawnedBot.DesignatedTarget;
        else
            spawnedBot.CurrentTarget = GetRandomAvailableBuilding(spawnedBot.DesignatedTarget);

        List<string> availableIds = new List<string>();

        foreach (KeyValuePair<string, bool> idPair in spawnData.m_botsInAction)
        {
            if(idPair.Value == false)
                availableIds.Add(idPair.Key);
        }

        spawnedBot.OnFinishedDelivery += OnBotFinishedDelivery;
        spawnedBot.Name = availableIds[Random.Range(0, availableIds.Count)];
        spawnedBot.OnDestroyed += OnBotDestroyed;
        OnBotSpawned?.Invoke(spawnedBot);
        spawnedBot.Init();
        
        m_currentSpawnDelay = Random.Range(m_currentLevel.m_spawnDelayMin, m_currentLevel.m_spawnDelayMax);
        
        spawnData.m_botsInAction[spawnedBot.Name] = true;

        m_spawnCount++;
        m_botsAlive++;
        if (m_spawnCount >= m_currentLevel.m_amountDroids)
            m_canSpawn = false;
    }

    private void OnBotDestroyed(DeliveryBot bot)
    {
        bot.spawnData.m_botsInAction[bot.Name] = false;
        bot.OnFinishedDelivery -= OnBotFinishedDelivery;
        m_botsAlive--;

        if (m_botsAlive <= 0 && m_spawnCount >= m_currentLevel.m_amountDroids)
        {
            OnAllBotsSpawnedAndDestroyed?.Invoke();
        }
    }

    public Building GetRandomAvailableBuilding(Building ignoredBuilding = null)
    {
        List<Building> availableBuildings = new List<Building>(m_buildingManager.Buildings);
        
        if (ignoredBuilding != null)
            availableBuildings.Remove(ignoredBuilding);

        int rand = Random.Range(0, availableBuildings.Count);

        return availableBuildings[rand];
    }
}
