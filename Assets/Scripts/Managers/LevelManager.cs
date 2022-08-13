using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    [SerializeField]
    private DeliveryTargetGenerator m_deliveryTargetGenerator;

    public DeliveryTargetGenerator DeliveryTargetGenerator => m_deliveryTargetGenerator;
    
    private void Awake()
    {
        Instance = this;
    }
}
