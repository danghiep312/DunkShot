using System;
using UnityEngine;

[System.Serializable]
public class Audio
{
    public string name;
    public AudioType audioType;
    public AudioClip clip;
    [Range(0f, 1f)] public float volume = 1;
    [Range(.1f, 3f)] public float pitch = 1;
    public bool loop;
    [HideInInspector] public AudioSource source;
}

public class AudioManager : Singleton<AudioManager>
{
    public Audio[] audios;

    public override void Awake()
    {
        base.Awake();
        foreach (Audio audio in audios)
        {
            audio.source = gameObject.AddComponent<AudioSource>();
            audio.source.clip = audio.clip;
            audio.source.volume = audio.volume;
            audio.source.pitch = audio.pitch;
            audio.source.loop = audio.loop;
        }
    }
    
    public void Play(string name)
    {
        Audio audio = Array.Find(audios, sound => sound.name == name);
        if (audio == null)
        {
            Common.LogWarning(this, "Can't find audio with name: " + name);
        }

        if (!GameManager.Instance.soundOn && audio.audioType == AudioType.Sound) return;
        audio.source.Play();
    }

    public void Stop(string name)
    {
        Audio audio = Array.Find(audios, sound => sound.name == name);
        if (audio == null)
        {
            Common.LogWarning(this, "Can't find audio with name: " + name);
        }
        
        audio.source.Stop();
    }
}
