using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DroidNameBinder : MonoBehaviour
{
    public DeliveryBot m_droid;

    private TextMeshProUGUI m_textMesh;

    private void Awake()
    {
        m_textMesh = GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        m_textMesh.text = m_droid.Name;
    }
}
