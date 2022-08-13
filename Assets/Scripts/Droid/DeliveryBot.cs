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
    
    private GameObject m_targetBuilding;
    public GameObject TargetBuilding => m_targetBuilding;

    private Pathfinding m_pathfinding;
    public Pathfinding Pathfinding => m_pathfinding;

    private Vector3 m_startPos;
    public Vector3 StartPos => m_startPos;

    private StateMachine m_stateMachine;

    private DeliveryTargetGenerator m_deliveryTargetGenerator;
    
    private void Awake()
    {
        m_pathfinding = FindObjectOfType<Pathfinding>();

        m_deliveryTargetGenerator = LevelManager.Instance.DeliveryTargetGenerator;

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

    private GameObject GetRandomAvailableBuilding()
    {
        List<GameObject> availableBuildings = m_deliveryTargetGenerator.GeneratedTargets;
        
        int rand = Random.Range(0, m_deliveryTargetGenerator.GeneratedTargets.Count);

        return availableBuildings[rand];
    }
}
