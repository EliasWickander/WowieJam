using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event<T> : ScriptableObject
{
    public delegate void OnInvokedDelegate(T value);
    
    public event OnInvokedDelegate OnInvoked;

    public void AddListener(OnInvokedDelegate method)
    {
        OnInvoked += method;
    }

    public void RemoveListener(OnInvokedDelegate method)
    {
        OnInvoked -= method;
    }
    
    public void Invoke(T value)
    {
        OnInvoked?.Invoke(value);
    }
}
