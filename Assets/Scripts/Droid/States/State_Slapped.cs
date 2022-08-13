using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Slapped : State
{
    private DeliveryBot m_controller;
    
    public State_Slapped(DeliveryBot controller) : base(controller.gameObject)
    {
        m_controller = controller;
    }

    public override event Action<Enum> onStateTransition;
    
    private float m_shockTimer = 0;
    
    public override void OnEnter(State prevState, object[] param)
    {
        m_shockTimer = 0;
        m_controller.CurrentTarget = m_controller.DesignatedTarget;
    }

    public override void OnTick()
    {
        if (m_shockTimer < m_controller.SlapShockTime)
        {
            m_shockTimer += Time.deltaTime;
        }
        else
        {
            m_shockTimer = 0;
            onStateTransition?.Invoke(DroidStates.Deliver);
        }
    }

    public override void OnExit(State nextState)
    {
        
    }
}
