using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(AudioSource))]
public class SlappyHand : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerMoveHandler
{
    private RectTransform m_rectTransform;
    private Vector2 m_startPos;

    private bool m_dragging = false;

    public LayerMask m_supervisorMask;
    public Camera m_supervisorCamera;

    public AudioClipData m_slapClip;
    public AudioClipData m_grabClip;
    public AudioClipData m_letGoClip;
    private AudioSource m_audioSource;
    
    
    private void Awake()
    {
        m_rectTransform = GetComponent<RectTransform>();

        m_audioSource = GetComponent<AudioSource>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        m_startPos = m_rectTransform.position;

        AudioManager.Instance.PlayAudio(m_audioSource, m_grabClip);
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
            AudioManager.Instance.PlayAudio(m_audioSource, m_slapClip);
            hitInfo.collider.GetComponent<DeliverySupervisor>().AskForSlap();
        }
        else
        {
            AudioManager.Instance.PlayAudio(m_audioSource, m_letGoClip);
        }
    }
}
