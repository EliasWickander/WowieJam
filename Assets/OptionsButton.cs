using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public abstract class CustomButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private Button m_button;
    public Button Button => m_button;

    public event Action<CustomButton> OnPressedCB;
    public event Action OnHoverEnterCB;
    public event Action OnHoverExitCB;

    public UnityEvent OnPressedEvent;
    public UnityEvent OnHoverEnterEvent;
    public UnityEvent OnHoverExitEvent;

    private bool m_enabled;

    private void Awake()
    {
        m_button = GetComponent<Button>();

        m_enabled = m_button.interactable;
    }
    
    protected virtual void OnPressed()
    {
        if(!m_enabled)
            return;
        
        OnHoverExit();
        
        OnPressedCB?.Invoke(this);
        OnPressedEvent?.Invoke();
    }

    protected virtual void OnHoverEnter()
    {
        if(!m_enabled)
            return;
        
        OnHoverEnterCB?.Invoke();
        OnHoverEnterEvent?.Invoke();
    }
    
    protected virtual void OnHoverExit()
    {
        if(!m_enabled)
            return;
        
        OnHoverExitCB?.Invoke();
        OnHoverExitEvent?.Invoke();
    }

    public void SetInteractable(bool active)
    {
        Button.interactable = active;
        m_enabled = active;
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        OnHoverEnter();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        OnHoverExit();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnPressed();
    }
}
public class OptionsButton : CustomButton
{

}
