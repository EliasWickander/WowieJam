using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Lighting Preset", menuName = "ScriptableObjects/LightingPreset")]
public class LightingPreset : ScriptableObject
{
    public Gradient m_ambientColor;
    public Gradient m_directionalColor;
    public Gradient m_fogColor;
}
