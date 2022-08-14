using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Malfunction : State
{
    private DeliveryBot m_controller;
    
    public State_Malfunction(DeliveryBot controller) : base(controller.gameObject)
    {
        m_controller = controller;
    }

    public override event Action<Enum> onStateTransition;
    
    private float m_shockTimer = 0;
    
    public override void OnEnter(State prevState, object[] param)
    {
        m_shockTimer = 0;
        m_controller.CurrentTarget = LevelManager.Instance.BotSpawner.GetRandomAvailableBuilding(m_controller.DesignatedTarget);
        AudioManager.Instance.PlayAudio(m_controller.m_audioSource, m_controller.m_botMalfunctioningClip, 0, m_controller.MalfunctionShockTime);

        m_controller.StartCoroutine(m_controller.StartSparks());
    }

    public override void OnTick()
    {
        if (m_shockTimer < m_controller.MalfunctionShockTime)
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
