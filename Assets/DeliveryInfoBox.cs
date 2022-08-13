using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeliveryInfoBox : MonoBehaviour
{
    public TextMeshProUGUI m_botNameAsset;
    public Image m_botImageAsset;
    public TextMeshProUGUI m_houseNumberAsset;
    
    public void Destroy()
    {
        Destroy(gameObject);
    }
}
