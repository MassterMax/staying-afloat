using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    float globalFXVolume = 0.2f;
    float globalMusicVolume = 0.2f;
    public SoundClip[] soundClips;
    public SoundClip bhClip;
    public SoundClip musicClip;
    void Awake()
    {
        foreach (SoundClip s in soundClips)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.audioClip;
            s.source.loop = false;
            // s.source.volume = globalFXVolume;
        }
        bhClip.source = gameObject.AddComponent<AudioSource>();
        bhClip.source.clip = bhClip.audioClip;
        bhClip.source.loop = true;

        musicClip.source = gameObject.AddComponent<AudioSource>();
        musicClip.source.clip = musicClip.audioClip;
        musicClip.source.loop = true;
        musicClip.source.volume = globalMusicVolume;
        musicClip.source.Play();
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

    public void PauseMusic()
    {
        musicClip.source.Pause();
    }

    public void PlayMusic()
    {
        musicClip.source.Play();
    }

    public void ChangeVolume(float volume, bool sfx)
    {
        if (sfx)
            globalFXVolume = volume;
        else
        {
            globalMusicVolume = volume;
            musicClip.source.volume = globalMusicVolume;
        }
    }

    public float GetVolume(bool sfx)
    {
        if (sfx) return globalFXVolume;
        return globalMusicVolume;
    }

    public void SetMusicPitch(float value)
    {
        musicClip.source.pitch = value;
    }
}
