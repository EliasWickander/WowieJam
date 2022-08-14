using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayedAudioData
{
    public AudioClipData clipData;
    public float duration;
    public float m_currentDuration = 0;
}
[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    public AudioSource m_audioSource;
    
    public static AudioManager Instance { get; private set; }

    private Dictionary<AudioSource, PlayedAudioData> m_activeSources = new Dictionary<AudioSource, PlayedAudioData>();

    public AudioGroupData m_masterGroup;
    private void Awake()
    {
        Instance = this;

        m_audioSource = GetComponent<AudioSource>();
        m_activeSources.Add(m_audioSource, null);
    }

    public void SetMasterVolume(float volume)
    {
        m_masterGroup.m_volume = Mathf.Clamp01(volume);

        foreach (KeyValuePair<AudioSource, PlayedAudioData> pair in m_activeSources)
        {
            if (pair.Key != null)
            {
                float groupVolume = Mathf.Clamp01(pair.Value.clipData.m_group.m_volume);

                float maxVolume = m_masterGroup.m_volume < groupVolume ? m_masterGroup.m_volume : groupVolume;
                    
                pair.Key.volume = Mathf.Lerp(0, maxVolume, volume);
            }
        }
    }
    
    public void PlayAudio(AudioClipData clip)
    {
        PlayAudio(m_audioSource, clip);
    }

    public void PlayAudio(AudioClipData clip, float delay = 0)
    {
        PlayAudio(m_audioSource, clip, delay);
    }

    private void Update()
    {
        foreach (KeyValuePair<AudioSource, PlayedAudioData> pair in m_activeSources)
        {
            if(pair.Key == null)
                continue;
            
            if (pair.Key.isPlaying && pair.Value.m_currentDuration < pair.Value.duration)
            {
                pair.Value.m_currentDuration += Time.deltaTime;
            }
            else
            {
                if(pair.Key.isPlaying)
                    pair.Key.Stop();
            }
        }
    }

    public void PlayAudio(AudioSource audioSource, AudioClipData clip, float delay = 0, float duration = Mathf.Infinity)
    {
        if(!m_activeSources.ContainsKey(audioSource))
            m_activeSources.Add(audioSource, null);

        m_activeSources[audioSource] = new PlayedAudioData();

        if (duration > clip.m_audioClip.length)
            m_activeSources[audioSource].duration = clip.m_audioClip.length;
        else
            m_activeSources[audioSource].duration = duration;
        
        m_activeSources[audioSource].clipData = clip;
        
        if (m_activeSources[audioSource].clipData)
            clip.m_group.OnVolumeChanged -= OnGroupVolumeChanged;
        
        m_activeSources[audioSource].m_currentDuration = 0;
        audioSource.clip = clip.m_audioClip;

        float groupVolume = Mathf.Clamp01(clip.m_group.m_volume);
        
        if (groupVolume > m_masterGroup.m_volume)
            audioSource.volume = m_masterGroup.m_volume;
        else
            audioSource.volume = m_masterGroup.m_volume - (m_masterGroup.m_volume - groupVolume);

        if(delay == 0)
            audioSource.Play();
        else
            audioSource.PlayDelayed(0);
        
        clip.m_group.OnVolumeChanged += OnGroupVolumeChanged;
    }
    public void OnGroupVolumeChanged(float volume, AudioGroupData data)
    {
        foreach (KeyValuePair<AudioSource, PlayedAudioData> pair in m_activeSources)
        {
            if (pair.Key != null)
            {
                if (pair.Value.clipData.m_group == data)
                {
                    float groupVolume = Mathf.Clamp01(pair.Value.clipData.m_group.m_volume);

                    float maxVolume = m_masterGroup.m_volume < groupVolume ? m_masterGroup.m_volume : groupVolume;
                    
                    pair.Key.volume = Mathf.Lerp(0, maxVolume, volume);
                }
            }
        }
    }
}
