using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    public Sprite m_icon;

    public GameObject m_highlightObject;
    public void SetHighlighted(bool enabled) 
    {
        m_highlightObject.SetActive(enabled);
    }
}
