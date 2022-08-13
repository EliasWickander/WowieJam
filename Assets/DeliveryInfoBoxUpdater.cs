using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryInfoBoxUpdater : MonoBehaviour
{
    [SerializeField] 
    private int m_maxBoxesAtOnce = 3;
    
    [SerializeField] 
    private DeliveryInfoBox m_infoBoxPrefab;
    
    private DeliveryBotSpawner m_deliveryBotSpawner;

    private Dictionary<DeliveryBot, DeliveryInfoBox> m_infoBoxesDisplayed = new Dictionary<DeliveryBot, DeliveryInfoBox>();
    
    private Queue<KeyValuePair<DeliveryBot, DeliveryInfoBox>> m_infoBoxesInQueue = new Queue<KeyValuePair<DeliveryBot, DeliveryInfoBox>>();
    private void Awake()
    {
        m_deliveryBotSpawner = FindObjectOfType<DeliveryBotSpawner>();

        m_deliveryBotSpawner.OnBotSpawned += OnBotSpawned;
    }

    private void OnBotSpawned(DeliveryBot bot)
    {
        bot.OnDestroyed += OnBotDestroyed;
        
        DeliveryInfoBox newInfoBox = Instantiate(m_infoBoxPrefab, transform);
        newInfoBox.m_botNameAsset.text = bot.Name;
        newInfoBox.m_botImageAsset.sprite = bot.DesignatedTarget.m_icon;

        if (m_infoBoxesDisplayed.Count < m_maxBoxesAtOnce)
        {
            m_infoBoxesDisplayed.Add(bot, newInfoBox);   
        }
        else
        {
            newInfoBox.gameObject.SetActive(false);
            m_infoBoxesInQueue.Enqueue(new KeyValuePair<DeliveryBot, DeliveryInfoBox>(bot, newInfoBox));   
        }
    }

    private void OnBotDestroyed(DeliveryBot bot)
    {
        DeliveryInfoBox infoBox = m_infoBoxesDisplayed[bot];

        m_infoBoxesDisplayed.Remove(bot);
        
        infoBox.Destroy();

        if (m_infoBoxesInQueue.Count > 0)
        {
            KeyValuePair<DeliveryBot, DeliveryInfoBox> newBoxPair = m_infoBoxesInQueue.Dequeue();
            newBoxPair.Value.gameObject.SetActive(true);
            m_infoBoxesDisplayed.Add(newBoxPair.Key, newBoxPair.Value);   
        }
    }
}
