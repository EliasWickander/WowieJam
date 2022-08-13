using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MistakeCheckBox : MonoBehaviour
{
    public int m_number = 1;

    public GameObject m_crossFillImage;

    private LevelManager m_levelManager;

    private void Awake()
    {
        m_levelManager = LevelManager.Instance;
        
        m_crossFillImage.SetActive(false);
    }

    private void OnEnable()
    {
        m_levelManager.OnMistakeAdded += OnMistakeAdded;
    }

    private void OnDisable()
    {
        m_levelManager.OnMistakeAdded -= OnMistakeAdded;
    }

    private void OnMistakeAdded()
    {
        if (m_levelManager.Mistakes == m_number)
        {
            m_crossFillImage.SetActive(true);   
        }
    }
}
