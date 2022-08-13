using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreCounter : MonoBehaviour
{
    public TextMeshProUGUI m_scoreTextAsset;

    private int m_currentScore = 0;
    private DeliveryBotSpawner m_botSpawner;

    private void Awake()
    {
        m_botSpawner = FindObjectOfType<DeliveryBotSpawner>();
        
        UpdateText();
    }

    private void OnEnable()
    {
        m_botSpawner.OnBotDelivered += OnBotDelivered;
    }

    private void OnDisable()
    {
        m_botSpawner.OnBotDelivered -= OnBotDelivered;
    }
    
    private void OnBotDelivered(DeliveryBot obj)
    {
        m_currentScore++;

        UpdateText();
    }

    private void UpdateText()
    {
        m_scoreTextAsset.text = "Score: " + m_currentScore;
    }
}
