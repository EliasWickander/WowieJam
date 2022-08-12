using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EventTrigger<T> : MonoBehaviour
{
    [SerializeField] 
    private Event<T> m_eventHolder = null;

    public void Trigger(T value)
    {
        if(m_eventHolder != null)
            m_eventHolder.Invoke(value);
    }
}
