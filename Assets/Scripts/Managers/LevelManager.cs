using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[Serializable]
public class LevelData
{
    public string m_levelName;
    public int m_amountDroids = 5;
    public float m_spawnDelayMin = 1;
    public float m_spawnDelayMax = 3;
    public float m_startDelay = 1;

    public List<BotSpawnData> m_availableBotsData;
}

[RequireComponent(typeof(AudioSource))]
public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    [SerializeField]
    private BuildingManager m_buildingManager;

    public BuildingManager BuildingManager => m_buildingManager;

    private DeliveryBotSpawner m_botSpawner;
    public DeliveryBotSpawner BotSpawner => m_botSpawner;
    public List<LevelData> m_levels;
    
    private NavGrid m_navGrid;

    public NavGrid NavGrid => m_navGrid;

    private int m_mistakes = 0;
    public int Mistakes => m_mistakes;

    private LevelData m_currentLevel = null;
    private int m_currentLevelIndex = -1;

    private LevelData m_nextLevel = null;
    public event Action OnMistakeAdded;
    public event Action<LevelData> OnLevelStart;

    private bool m_transitioningToNextLevel = false;
    private float m_transitionTimer = 0;

    public AudioClipData m_finishedLevelClip;
    public AudioClipData m_botMistakeClip;

    private AudioSource m_audioSource;

    private AudioClipData m_currentClip;

    public bool m_isPaused = false;

    public bool m_allLevelsFinished = false;

    public GameObject m_gameoverCanvas;
    public GameObject m_winCanvas;
    
    private void Awake()
    {
        Instance = this;

        m_botSpawner = FindObjectOfType<DeliveryBotSpawner>();
        m_navGrid = FindObjectOfType<NavGrid>();
        m_mistakes = 0;

        m_audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        TransitionToNextLevel();
    }

    private void OnEnable()
    {
        m_botSpawner.OnAllBotsSpawnedAndDestroyed += OnAllBotsSpawnedAndDestroyed;
    }

    private void OnDisable()
    {
        m_botSpawner.OnAllBotsSpawnedAndDestroyed -= OnAllBotsSpawnedAndDestroyed;
    }

    private void Update()
    {
        if (m_transitioningToNextLevel)
        {
            if (m_transitionTimer < m_nextLevel.m_startDelay)
            {
                m_transitionTimer += Time.deltaTime;
            }
            else
            {
                m_transitionTimer = 0;
                NextLevel();
            }
        }
    }

    private void OnAllBotsSpawnedAndDestroyed()
    {
        Debug.Log("finished level " + m_currentLevel.m_levelName);
        TransitionToNextLevel();
    }

    public void AddMistake()
    {
        m_mistakes++;

        OnMistakeAdded?.Invoke();

        if (m_mistakes < 3)
        {
            AudioManager.Instance.PlayAudio(m_audioSource, m_botMistakeClip);
        }
        else
        {
            GameManager.Instance.SetState(GameStateType.GameState_Lost);

            if (m_gameoverCanvas != null)
            {
                m_isPaused = true;
                m_gameoverCanvas.SetActive(true);   
            }
            //game over 
        }
    }

    public void TransitionToNextLevel()
    {
        if (m_levels.Count > m_currentLevelIndex + 1)
        {
            m_nextLevel = m_levels[m_currentLevelIndex + 1];
            m_transitionTimer = 0;
            m_transitioningToNextLevel = true;   
            
            if(m_currentLevelIndex != -1)
                AudioManager.Instance.PlayAudio(m_audioSource, m_finishedLevelClip);
        }
        else
        {
            GameManager.Instance.SetState(GameStateType.GameState_Win);
            Debug.Log("no more levels");
            //no more levels
            m_allLevelsFinished = true;
            
            if(m_winCanvas != null)
                m_winCanvas.SetActive(true);
        }
    }
    
    public void NextLevel()
    {
        Debug.Log("start level " + m_nextLevel.m_levelName);
        m_transitioningToNextLevel = false;
        m_currentLevelIndex++;
        m_currentLevel = m_nextLevel;   
        OnLevelStart?.Invoke(m_currentLevel);
    }
}
