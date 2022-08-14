using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

[Serializable]
public class GameState
{
    public GameStateType m_type;
    public string m_sceneToOpenOnEnter = "";
    public AudioClipData m_soundTrack;
    public float m_soundStartDelay = 0;
}

public enum GameStateType
{
    GameState_Menu,
    GameState_Playing,
    GameState_Tutorial,
    GameState_Lost,
    GameState_Win,
}

public class GameManager : MonoBehaviour
{
    public List<GameState> m_gameStates;

    private GameState m_currentGameState;
    public GameState CurrentGameState => m_currentGameState;
    
    public Dictionary<GameStateType, GameState> m_gameStatesDictionary = new Dictionary<GameStateType, GameState>();
    
    public static GameManager Instance { get; private set; }

    public GameStateType m_startGameState;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        Screen.SetResolution(1920, 1080, FullScreenMode.FullScreenWindow);
        foreach (GameState state in m_gameStates)
        {
            m_gameStatesDictionary.Add(state.m_type, state);
        }
        
        SetState(m_startGameState);
    }

    public void SetState(GameStateType state)
    {
        m_currentGameState = m_gameStatesDictionary[state];

        if(m_currentGameState.m_soundTrack != null)
            AudioManager.Instance.PlayAudio(m_currentGameState.m_soundTrack, m_currentGameState.m_soundStartDelay);
        
        if (m_currentGameState.m_sceneToOpenOnEnter != "" && SceneManager.GetActiveScene().name != m_currentGameState.m_sceneToOpenOnEnter)
        {
            SceneManager.LoadScene(m_currentGameState.m_sceneToOpenOnEnter);
        }
    }
}
