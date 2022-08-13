using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_End : State
{
    public State_End(DeliveryBot controller) : base(controller.gameObject)
    {
    }

    public override event Action<Enum> onStateTransition;
    public override void OnEnter(State prevState, object[] param)
    {
        Debug.Log("Done");
    }

    public override void OnTick()
    {
        
    }

    public override void OnExit(State nextState)
    {
        
    }
}
