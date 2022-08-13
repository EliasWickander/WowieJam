using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliverySupervisor : MonoBehaviour
{
    public DroidType m_supervisedBotType;

    private DeliveryBotSpawner m_deliveryBotSpawner;

    private List<DeliveryBot> m_supervisedBots = new List<DeliveryBot>();
    
    private void Awake()
    {
        m_deliveryBotSpawner = FindObjectOfType<DeliveryBotSpawner>();

        m_deliveryBotSpawner.OnBotSpawned += OnBotSpawned;
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
        
        LevelManager.Instance.AddMistake();
        //No bots to correct
    }
}
