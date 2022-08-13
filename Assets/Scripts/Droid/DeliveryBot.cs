using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

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
    
    private Building m_targetBuilding;
    public Building TargetBuilding => m_targetBuilding;

    private Pathfinding m_pathfinding;
    public Pathfinding Pathfinding => m_pathfinding;

    private Vector3 m_startPos;
    public Vector3 StartPos => m_startPos;

    private StateMachine m_stateMachine;

    private BuildingManager m_deliveryTargetGenerator;
    
    private void Awake()
    {
        m_pathfinding = FindObjectOfType<Pathfinding>();

        m_deliveryTargetGenerator = LevelManager.Instance.BuildingManager;

        m_targetBuilding = GetRandomAvailableBuilding();
    }

    private void OnEnable()
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
        };
        
        m_stateMachine = new StateMachine(states);
    }

    private Building GetRandomAvailableBuilding()
    {
        List<Building> availableBuildings = m_deliveryTargetGenerator.Buildings;
        
        int rand = Random.Range(0, m_deliveryTargetGenerator.Buildings.Count);

        return availableBuildings[rand];
    }
}
