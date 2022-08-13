using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Deliver : State
{
    private DeliveryBot m_controller;
    
    private List<PathNode> m_path = null;

    private int m_currentNodeIndex = -1;
    
    public State_Deliver(DeliveryBot controller) : base(controller.gameObject)
    {
        m_controller = controller;
    }

    public override event Action<Enum> onStateTransition;
    public override void OnEnter(State prevState, object[] param)
    {
        m_path = m_controller.Pathfinding.FindPath(m_controller.transform.position, m_controller.TargetBuilding.transform.position, false);
        NextNode();
    }

    public override void OnTick()
    {
        Vector3 dirToNode = m_path[m_currentNodeIndex].WorldPosition - m_controller.transform.position;
        dirToNode.y = 0;

        if (dirToNode.magnitude < 0.2f)
        {
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
