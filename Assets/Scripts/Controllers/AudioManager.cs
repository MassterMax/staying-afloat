using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    float globalFXVolume = 0.2f;
    public SoundClip[] soundClips;
    void Awake()
    {
        foreach (SoundClip s in soundClips)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.audioClip;
            // s.source.volume = globalFXVolume;
        }
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
}
