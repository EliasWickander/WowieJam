using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(AudioSource))]
public class DeliverySupervisor : MonoBehaviour
{
    public DroidType m_supervisedBotType;

    private DeliveryBotSpawner m_deliveryBotSpawner;

    private List<DeliveryBot> m_supervisedBots = new List<DeliveryBot>();

    private AudioSource m_audioSource;
    public AudioClipData m_angryClip;
    public AudioClipData m_idleClip;

    public float m_twitchDelayMin = 1;
    public float m_twitchDelayMax = 3;

    private float m_twitchTimer = 0;
    private float m_currentTwitchTime;
    
    private void Awake()
    {
        m_audioSource = GetComponent<AudioSource>();
        m_deliveryBotSpawner = FindObjectOfType<DeliveryBotSpawner>();

        m_deliveryBotSpawner.OnBotSpawned += OnBotSpawned;

        m_currentTwitchTime = Random.Range(m_twitchDelayMin, m_twitchDelayMax);
    }

    private void OnBotSpawned(DeliveryBot bot)
    {
        if (bot.DroidType == m_supervisedBotType)
        {
            m_supervisedBots.Add(bot);

            bot.OnDestroyed += OnBotDestroyed;
        }
    }

    private void OnBotDestroyed(DeliveryBot bot)
    {
        m_supervisedBots.Remove(bot);
    }

    private void Update()
    {
        if (GameManager.Instance.CurrentGameState ==
            GameManager.Instance.m_gameStatesDictionary[GameStateType.GameState_Playing])
        {
            if (m_twitchTimer < m_currentTwitchTime)
            {
                m_twitchTimer += Time.deltaTime;
            }
            else
            {
                StartTwitch();
                m_twitchTimer = 0;
            }
        }
    }

    private void StartTwitch()
    {
        m_currentTwitchTime = Random.Range(m_twitchDelayMin, m_twitchDelayMax);
        
        AudioManager.Instance.PlayAudio(m_audioSource, m_idleClip);
    }
    
    public void AskForSlap()
    {
        foreach (DeliveryBot bot in m_supervisedBots)
        {
            if (Equals(bot.StateMachine.CurrentStateType, DroidStates.Deliver) &&
                bot.CurrentTarget != bot.DesignatedTarget)
            {
                bot.Slap();
                return;
            }
        }
        
        AudioManager.Instance.PlayAudio(m_audioSource, m_angryClip);
        LevelManager.Instance.AddMistake();
        //No bots to correct
    }
}
