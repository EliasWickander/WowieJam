using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class State_Deliver : State
{
    private DeliveryBot m_controller;
    
    public List<PathNode> m_path = null;

    public int m_currentNodeIndex = -1;

    private int m_malfunctionNode = -1;
    private bool m_willMalfunction = false;

    private float m_transitionTimer = 0;
    private bool m_isGettingSlapped = false;
    
    public State_Deliver(DeliveryBot controller) : base(controller.gameObject)
    {
        m_controller = controller;
    }

    public override event Action<Enum> onStateTransition;

    private bool m_firstDeliver = false;
    public override void OnEnter(State prevState, object[] param)
    {
        m_isGettingSlapped = false;
        m_transitionTimer = 0;
        m_controller.OnSlapped += OnSlapped;

        m_currentNodeIndex = -1;
        
        NavNode targetNode = m_controller.CurrentTarget.ClosestNavNode;
        
        m_path = m_controller.Pathfinding.FindPath(m_controller.transform.position, targetNode.WorldPosition);
        
        if (!m_controller.WasSlapped && m_controller.DesignatedTarget == m_controller.CurrentTarget)
        {
            if (Random.Range(0.0f, 1.0f) < LevelManager.Instance.BotSpawner.m_malfunctionPercentage)
            {
                m_malfunctionNode = Random.Range((int)(m_path.Count * 0.5f) - 1, m_path.Count);
                m_willMalfunction = true;
            }
        }

        if (!m_firstDeliver)
        {
            m_firstDeliver = true;
            AudioManager.Instance.PlayAudio(m_controller.m_audioSource, m_controller.m_botDepartingClip);
        }

        NextNode();
    }

    public override void OnTick()
    {
        if (m_isGettingSlapped)
        {
            if (m_transitionTimer < 0.5f)
            {
                m_transitionTimer += Time.deltaTime;
            }
            else
            {
                m_transitionTimer = 0;
                onStateTransition?.Invoke(DroidStates.Slapped);
            }
        }
        
        if(LevelManager.Instance.m_isPaused)
            return;
        
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
        if (nextState == m_controller.StateMachine.m_states[DroidStates.Return] &&
            m_controller.CurrentTarget != m_controller.DesignatedTarget)
        {
            LevelManager.Instance.AddMistake();
        }
        m_controller.OnSlapped -= OnSlapped;
        m_willMalfunction = false;
        m_malfunctionNode = -1;
    }
    
    private void OnSlapped()
    {
        m_isGettingSlapped = true;
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
