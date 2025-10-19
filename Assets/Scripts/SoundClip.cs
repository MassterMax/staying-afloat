using UnityEngine;

[System.Serializable]
public class SoundClip
{
    public string name;
    public AudioClip audioClip;

    [HideInInspector]
    public AudioSource source;
}
