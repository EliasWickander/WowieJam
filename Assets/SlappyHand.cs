using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SlappyHand : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerMoveHandler
{
    private RectTransform m_rectTransform;
    private Vector2 m_startPos;

    private bool m_dragging = false;

    public LayerMask m_supervisorMask;
    public Camera m_supervisorCamera;
    
    private void Awake()
    {
        m_rectTransform = GetComponent<RectTransform>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        m_startPos = m_rectTransform.position;

        m_dragging = true;
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        if(!m_dragging)
            return;
        
        m_rectTransform.position = eventData.position;
    }
    
    public void OnPointerUp(PointerEventData eventData)
    {
        m_rectTransform.position = m_startPos;

        m_dragging = false;

        Ray ray = m_supervisorCamera.ScreenPointToRay(eventData.position);

        if (Physics.Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity, m_supervisorMask))
        {
            hitInfo.collider.GetComponent<DeliverySupervisor>().AskForSlap();
        }
    }
}
