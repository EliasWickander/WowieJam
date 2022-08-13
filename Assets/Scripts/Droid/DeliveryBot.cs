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
    End,
}

public enum DroidType
{
    Red,
    Green,
    Blue
}
public class DeliveryBot : MonoBehaviour
{
    [HideInInspector]
    public BotSpawnData spawnData;
    [SerializeField] 
    private string m_prefix;

    [SerializeField] 
    private DroidType m_type;
    
    [SerializeField] 
    private float m_moveSpeed = 5;

    [SerializeField] 
    private float m_slapShockTime = 1;

    public float SlapShockTime => m_slapShockTime;

    public string Prefix => m_prefix;
    
    public string Name { get; set; }

    public DroidType DroidType => m_type;
    public float MoveSpeed => m_moveSpeed;
    public Building DesignatedTarget { get; set; }
    public Building CurrentTarget { get; set; }

    private Pathfinding m_pathfinding;
    public Pathfinding Pathfinding => m_pathfinding;

    private Vector3 m_startPos;
    public Vector3 StartPos => m_startPos;

    private StateMachine m_stateMachine;
    public StateMachine StateMachine => m_stateMachine;

    public event Action<DeliveryBot> OnDestroyed;

    public event Action OnSlapped;

    private void Awake()
    {
        m_pathfinding = FindObjectOfType<Pathfinding>();
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
            {DroidStates.Slapped, new State_Slapped(this)}
        };
        
        m_stateMachine = new StateMachine(states);
    }

    public void Slap()
    {
        OnSlapped?.Invoke();
    }
    
    public void Destroy()
    {
        OnDestroyed?.Invoke(this);
        Destroy(gameObject);
    }
}
