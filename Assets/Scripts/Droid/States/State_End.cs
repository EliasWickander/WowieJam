using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_End : State
{
    private DeliveryBot m_controller;
    
    public State_End(DeliveryBot controller) : base(controller.gameObject)
    {
        m_controller = controller;
    }

    public override event Action<Enum> onStateTransition;
    public override void OnEnter(State prevState, object[] param)
    {
        m_controller.Destroy();
    }

    public override void OnTick()
    {
        
    }

    public override void OnExit(State nextState)
    {
        
    }
}
