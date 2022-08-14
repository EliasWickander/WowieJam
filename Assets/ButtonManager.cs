using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonManager : MonoBehaviour
{
    private CustomButton[] m_buttons;

    private CustomButton m_currentButton = null;
    
    private void Awake()
    {
        m_buttons = FindObjectsOfType<CustomButton>();
        
    }

    private void OnEnable()
    {
        foreach (CustomButton button in m_buttons)
        {
            button.OnPressedCB += OnButtonPressed;
        }
    }

    private void OnDisable()
    {
        foreach (CustomButton button in m_buttons)
        {
            button.OnPressedCB -= OnButtonPressed;
        }
    }

    private void OnButtonPressed(CustomButton button)
    {
        if (m_currentButton != null)
        {
            m_currentButton.SetInteractable(true);
        }

        m_currentButton = button;
        m_currentButton.SetInteractable(false);
    }
}
