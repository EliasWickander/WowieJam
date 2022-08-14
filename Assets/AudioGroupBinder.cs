using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioGroupBinder : MonoBehaviour
{
    public AudioGroupData m_group;

    private Slider m_slider;

    private void Awake()
    {
        m_slider = GetComponent<Slider>();

        m_slider.value = m_group.m_volume;
    }
}
