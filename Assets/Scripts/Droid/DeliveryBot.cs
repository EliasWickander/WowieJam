using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DroidStates
{
    Deliver,
    Return,
    End,
}

public class DeliveryBot : MonoBehaviour
{
    [SerializeField] 
    private float m_moveSpeed = 5;

    public float MoveSpeed => m_moveSpeed;
    
    public Transform m_target;

    private Pathfinding m_pathfinding;
    public Pathfinding Pathfinding => m_pathfinding;

    private Vector3 m_startPos;
    public Vector3 StartPos => m_startPos;

    private StateMachine m_stateMachine;
    
    private void Awake()
    {
        m_startPos = transform.position;
        m_pathfinding = FindObjectOfType<Pathfinding>();
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
        };
        
        m_stateMachine = new StateMachine(states);
    }
}
