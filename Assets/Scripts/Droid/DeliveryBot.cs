using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public enum DroidStates
{
    Deliver,
    Return,
    Slapped,
    Malfunction,
    End,
}

public enum DroidType
{
    Red,
    Green,
    Blue
}

[RequireComponent(typeof(AudioSource))]
public class DeliveryBot : MonoBehaviour
{
    [HideInInspector]
    public BotSpawnData spawnData;
    
    [SerializeField] 
    private string m_prefix;
    
    [SerializeField] 
    private float m_moveSpeed = 5;

    [SerializeField] 
    private float m_slapShockTime = 1;
    
    private float m_malfunctionShockTime = 1;

    [SerializeField] 
    private GameObject m_highlightObject;

    [SerializeField] 
    private DroidType m_type;

    public DroidType DroidType => m_type;
    
    public float SlapShockTime => m_slapShockTime;
    public float MalfunctionShockTime => m_malfunctionShockTime;

    public string Prefix => m_prefix;
    
    public string Name { get; set; }

    public float MoveSpeed => m_moveSpeed;
    public Building DesignatedTarget { get; set; }
    public Building CurrentTarget { get; set; }

    private Pathfinding m_pathfinding;
    public Pathfinding Pathfinding => m_pathfinding;

    private Vector3 m_startPos;
    public Vector3 StartPos => m_startPos;

    private StateMachine m_stateMachine;
    public StateMachine StateMachine => m_stateMachine;

    public AudioClipData m_aiSorryClip;
    public AudioClipData m_botDepartingClip;
    public AudioClipData m_botMalfunctioningClip;

    public GameObject m_sparkVFX;

    public event Action<DeliveryBot> OnDestroyed;

    public event Action OnSlapped;

    public event Action<DeliveryBot> OnFinishedDelivery;

    public bool WasSlapped { get; set; }

    public GameObject m_exclamationMarkObject = null;
    
    [HideInInspector]
    public AudioSource m_audioSource;

    private void Awake()
    {
        m_pathfinding = FindObjectOfType<Pathfinding>();
        m_audioSource = GetComponent<AudioSource>();
    }
    
    public void Init()
    {
        m_startPos = transform.position;
        InitStateMachine();
    }
    
    private void Update()
    {
        m_stateMachine.Update();
    }

    private void InitStateMachine()
    {
        Dictionary<Enum, State> states = new Dictionary<Enum, State>()
        {
            {DroidStates.Deliver, new State_Deliver(this)},
            {DroidStates.Return, new State_Return(this)},
            {DroidStates.End, new State_End(this)},
            {DroidStates.Slapped, new State_Slapped(this)},
            {DroidStates.Malfunction, new State_Malfunction(this)}
        };
        
        m_stateMachine = new StateMachine(states);
    }

    public void Slap()
    {
        OnSlapped?.Invoke();
    }

    public void SetHighlighted(bool enabled)
    {
        m_highlightObject.SetActive(enabled);
    }

    public void FinishedDeliveryCallback()
    {
        OnFinishedDelivery?.Invoke(this);
    }
    
    public void Destroy()
    {
        OnDestroyed?.Invoke(this);
        Destroy(gameObject);
    }

    public IEnumerator StartSparks()
    {
        m_sparkVFX.SetActive(true);
        yield return new WaitForSeconds(m_malfunctionShockTime);
        m_sparkVFX.SetActive(false);
    }
}
