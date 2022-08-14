using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class State_Deliver : State
{
    private DeliveryBot m_controller;
    
    private List<PathNode> m_path = null;

    private int m_currentNodeIndex = -1;

    private int m_malfunctionNode = -1;
    private bool m_willMalfunction = false;
    
    public State_Deliver(DeliveryBot controller) : base(controller.gameObject)
    {
        m_controller = controller;
    }

    public override event Action<Enum> onStateTransition;
    public override void OnEnter(State prevState, object[] param)
    {
        m_controller.OnSlapped += OnSlapped;

        m_currentNodeIndex = -1;
        
        NavNode targetNode = m_controller.CurrentTarget.ClosestNavNode;
        
        m_path = m_controller.Pathfinding.FindPath(m_controller.transform.position, targetNode.WorldPosition);

        if (!m_controller.WasSlapped && m_controller.DesignatedTarget == m_controller.CurrentTarget)
        {
            m_malfunctionNode = Random.Range((int)(m_path.Count * 0.5f) - 1, m_path.Count);
            m_willMalfunction = true;
        }
        
        NextNode();
    }

    public override void OnTick()
    {
        Vector3 dirToNode = m_path[m_currentNodeIndex].WorldPosition - m_controller.transform.position;
        dirToNode.y = 0;

        if (dirToNode.magnitude < 0.2f)
        {
            if (m_currentNodeIndex == m_malfunctionNode)
            {
                onStateTransition?.Invoke(DroidStates.Malfunction);
                return;
            }
            
            if (!NextNode())
            {
                if (m_controller.CurrentTarget != m_controller.DesignatedTarget)
                {
                    LevelManager.Instance.AddMistake();
                }
                else
                {
                    m_controller.FinishedDeliveryCallback();
                }
                
                onStateTransition?.Invoke(DroidStates.Return);
                return;
            }
        }
        else
        {
            dirToNode.Normalize();
            m_controller.transform.position += dirToNode * m_controller.MoveSpeed * Time.deltaTime;
            m_controller.transform.rotation = Quaternion.LookRotation(dirToNode);
        }
    }

    public override void OnExit(State nextState)
    {
        m_controller.OnSlapped -= OnSlapped;
        m_willMalfunction = false;
        m_malfunctionNode = -1;
    }
    
    private void OnSlapped()
    {
        onStateTransition?.Invoke(DroidStates.Slapped);
    }
    
    private bool NextNode()
    {
        if (m_currentNodeIndex + 1 < m_path.Count)
        {
            m_currentNodeIndex++;
            return true;
        }

        return false;
    }
}
