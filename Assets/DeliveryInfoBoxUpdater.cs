using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class DeliveryInfoBoxUpdater : MonoBehaviour
{
    [SerializeField] 
    private int m_maxBoxesAtOnce = 3;
    
    [SerializeField] 
    private DeliveryInfoBox m_infoBoxPrefab;
    
    private DeliveryBotSpawner m_deliveryBotSpawner;

    private Dictionary<DeliveryBot, DeliveryInfoBox> m_infoBoxesDisplayed = new Dictionary<DeliveryBot, DeliveryInfoBox>();
    
    private Dictionary<DeliveryBot, DeliveryInfoBox> m_infoBoxesInQueue = new Dictionary<DeliveryBot, DeliveryInfoBox>();

    private AudioSource m_audioSource;
    public AudioClipData m_newOrderClip;
    
    private void Awake()
    {
        m_deliveryBotSpawner = FindObjectOfType<DeliveryBotSpawner>();
        m_audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        m_deliveryBotSpawner.OnBotSpawned += OnBotSpawned;
    }

    private void OnDisable()
    {
        m_deliveryBotSpawner.OnBotSpawned -= OnBotSpawned;
    }

    private void OnBotSpawned(DeliveryBot bot)
    {
        bot.OnDestroyed += OnBotDestroyed;
        
        DeliveryInfoBox newInfoBox = Instantiate(m_infoBoxPrefab, transform);
        newInfoBox.m_botNameAsset.text = bot.Name;
        newInfoBox.m_botImageAsset.sprite = bot.DesignatedTarget.m_icon;
        newInfoBox.m_houseNumberAsset.text = bot.DesignatedTarget.m_houseNumber;

        if (m_infoBoxesDisplayed.Count < m_maxBoxesAtOnce)
        {
            m_infoBoxesDisplayed.Add(bot, newInfoBox);   
            AudioManager.Instance.PlayAudio(m_audioSource, m_newOrderClip);
        }
        else
        {
            newInfoBox.gameObject.SetActive(false);
            m_infoBoxesInQueue.Add(bot, newInfoBox);   
        }
    }

    private void OnBotDestroyed(DeliveryBot bot)
    {
        bot.OnDestroyed -= OnBotDestroyed;
        
        if (m_infoBoxesDisplayed.ContainsKey(bot))
        {
            DeliveryInfoBox infoBox = m_infoBoxesDisplayed[bot];

            m_infoBoxesDisplayed.Remove(bot);
        
            infoBox.Destroy();   
        }
        else
        {
            DeliveryInfoBox infoBox = m_infoBoxesInQueue[bot];

            m_infoBoxesInQueue.Remove(bot);
        
            infoBox.Destroy();   
        }

        if (m_infoBoxesInQueue.Count > 0 && m_infoBoxesDisplayed.Count < m_maxBoxesAtOnce)
        {
            KeyValuePair<DeliveryBot, DeliveryInfoBox> newBoxPair = m_infoBoxesInQueue.First();
            newBoxPair.Value.gameObject.SetActive(true);
            m_infoBoxesDisplayed.Add(newBoxPair.Key, newBoxPair.Value);
            m_infoBoxesInQueue.Remove(newBoxPair.Key);
            AudioManager.Instance.PlayAudio(m_audioSource, m_newOrderClip);
        }
    }
}
