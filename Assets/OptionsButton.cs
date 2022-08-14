using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public abstract class CustomButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private Button m_button;
    public Button Button => m_button;

    private void Awake()
    {
        m_button = GetComponent<Button>();
    }
    
    protected virtual void OnPressed()
    {
        
    }

    protected virtual void OnHoverEnter()
    {
        
    }
    
    protected virtual void OnHoverExit()
    {
        
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
public class OptionsButton : Button
{

}
