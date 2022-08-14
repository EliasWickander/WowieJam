using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Audio Group Data", menuName = "ScriptableObjects/AudioGroupData")]
public class AudioGroupData : ScriptableObject
{
    public float m_volume;
    public event Action<float, AudioGroupData> OnVolumeChanged;

    public void SetVolume(float volume)
    {
        m_volume = volume;
        OnVolumeChanged?.Invoke(volume, this);
    }
}
