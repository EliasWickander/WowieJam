using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class TutorialSegment
{
    public UnityEvent m_event;
    public bool m_automatic = false;
    public float m_autoDuration = 0;
}

public class TutorialSequence : MonoBehaviour
{
    public List<TutorialSegment> m_segments = new List<TutorialSegment>();

    private TutorialSegment m_currentSegment = null;

    private int m_currentSegmentIndex = -1;

    private float m_segmentStartTimeStamp;
    private bool m_done = false;

    private DeliveryBotSpawner m_spawner;

    private int m_spawnedBots = 0;
    private DeliveryBot m_lastBotSpawned;

    private void Awake()
    {
        m_spawner = FindObjectOfType<DeliveryBotSpawner>();
        StartSequence();

        LevelManager.Instance.BotSpawner.m_canSpawn = false;
    }

    private void Start()
    {
        LevelManager.Instance.BotSpawner.m_malfunctionPercentage = 0;
        LevelManager.Instance.BotSpawner.m_correctAtStartPercentage = 1;
    }

    private void OnEnable()
    {
        m_spawner.OnBotSpawned += OnBotSpawned;
    }

    private void OnDisable()
    {
        m_spawner.OnBotSpawned -= OnBotSpawned;
    }

    private void OnBotSpawned(DeliveryBot bot)
    {
        m_spawnedBots++;
        m_lastBotSpawned = bot;
        LevelManager.Instance.BotSpawner.m_canSpawn = false;
        bot.OnDestroyed += OnBotDestroyed;
    }

    private void Update()
    {
        if(m_done)
            return;

        switch (m_currentSegmentIndex)
        {
            case 0:
            {
                if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
                {
                    ManualNextSegment();
                }
                break;
            }
            case 1:
            {
                Debug.Log(LevelManager.Instance.BotSpawner.m_canSpawn);
                if (Time.time >= m_segmentStartTimeStamp + m_currentSegment.m_autoDuration)
                {
                    NextSegment();
                }
                break;
            }
            case 2:
            {
                if (m_spawnedBots == 1)
                {
                    if (m_lastBotSpawned.m_isHighlighted)
                    {
                        LevelManager.Instance.BotSpawner.m_malfunctionPercentage = 1;
                        LevelManager.Instance.BotSpawner.m_correctAtStartPercentage = 0;
                        
                        NextSegment();
                    }
                }
                break;
            }
            case 3:
            {
                if (m_spawnedBots == 2)
                {
                    NextSegment();   
                }

                break;
            }
            case 4:
            {
                if (Time.time >= m_segmentStartTimeStamp + m_currentSegment.m_autoDuration)
                {
                    NextSegment();
                }
                
                break;
            }
            case 5:
            {
                if (m_lastBotSpawned.WasSlapped)
                {
                    NextSegment();
                }
                break;
            }
            case 6:
            {
                if (Time.time >= m_segmentStartTimeStamp + m_currentSegment.m_autoDuration)
                {
                    NextSegment();
                }

                break;
            }
            case 7:
            {
                GameManager.Instance.SetState(GameStateType.GameState_Menu);
                break;
            }
        }
    }
    
    public void OnBotDestroyed(DeliveryBot bot)
    {
        bot.OnDestroyed -= OnBotDestroyed;
        LevelManager.Instance.BotSpawner.m_canSpawn = true;
    }
    
    public void StartSequence()
    {
        m_currentSegmentIndex = -1;
        NextSegment();
    }

    public void NextSegment()
    {
        if (m_currentSegmentIndex + 1 < m_segments.Count)
        {
            m_currentSegmentIndex++;
            m_currentSegment = m_segments[m_currentSegmentIndex];   
            m_currentSegment.m_event?.Invoke();
            m_segmentStartTimeStamp = Time.unscaledTime;
        }
        else
        {
            //done
            m_done = true;
        }
    }

    public void ManualNextSegment()
    {
        if (m_currentSegmentIndex == 0)
        {
            LevelManager.Instance.BotSpawner.m_canSpawn = true;
            NextSegment();
        }
    }

    public void SetPaused(bool paused)
    {
        LevelManager.Instance.m_isPaused = paused;
    }
}
