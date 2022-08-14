using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Audio Clip", menuName = "ScriptableObjects/AudioClipData")]
public class AudioClipData : ScriptableObject
{
    public AudioClip m_audioClip;
    public AudioGroupData m_group;
}
