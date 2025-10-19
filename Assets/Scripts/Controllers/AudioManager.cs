using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    float globalFXVolume = 0.2f;
    public SoundClip[] soundClips;
    public SoundClip bhClip;
    void Awake()
    {
        foreach (SoundClip s in soundClips)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.audioClip;
            // s.source.volume = globalFXVolume;
        }
        bhClip.source = gameObject.AddComponent<AudioSource>();
        bhClip.source.clip = bhClip.audioClip;
        bhClip.source.loop = true;
    }

    public void Play(string name)
    {
        SoundClip s = Array.Find(soundClips, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogError(name);
            return;
        }
        s.source.volume = globalFXVolume;
        s.source.Play();
    }

    public void PlayBH()
    {
        if (bhClip.source.isPlaying) return;
        bhClip.source.volume = globalFXVolume;
        bhClip.source.Play();
    }

    public void PauseBH()
    {
        bhClip.source.Pause();
    }
}
