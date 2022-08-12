using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Variable<T> : ScriptableObject
{
    public delegate void OnValueChangedCallback(T oldValue, T newValue);

    public event OnValueChangedCallback OnValueChanged;
    
    private T m_value;

    public T Value
    {
        get
        {
            return m_value;
        }
        set
        {
            var oldValue = Value;
            m_value = value;
            
            OnValueChanged?.Invoke(oldValue, m_value);
        }
    }
}
