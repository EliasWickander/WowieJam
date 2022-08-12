using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class EventListener<TType, TEvent> : MonoBehaviour where TEvent : Event<TType>
{
    public TEvent m_eventHolder = null;

    public UnityEvent<TType> m_event;
    
    private void OnEnable()
    {
        if (m_eventHolder != null)
            m_eventHolder.OnInvoked += OnInvoked;
    }

    private void OnDisable()
    {
        if (m_eventHolder != null)
            m_eventHolder.OnInvoked -= OnInvoked;
    }

    private void OnInvoked(TType value)
    {
        m_event?.Invoke(value);
    }
}
