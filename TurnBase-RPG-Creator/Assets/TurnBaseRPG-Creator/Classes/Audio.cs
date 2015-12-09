using UnityEngine;
using System;

[Serializable]
public class Audio
{
    public GameObject gameobject;
    public AudioSource Source;
    
    private Constant.AudioType type;

    public Audio()
    {
        gameobject = new GameObject("BackgroundAudio");
    }    

    public void CreateAudioSource(AudioClip clip)
    {
        Source = gameobject.AddComponent<AudioSource>();
        Source.clip = clip;
        GetAudioType(clip.name);

        switch (type)
        {
            case Constant.AudioType.Background:
                Source.loop = true;
                Source.volume = 0.5f;
                break;
            case Constant.AudioType.BackgroundEffect:
                Source.volume = 0.7f;
                break;
            case Constant.AudioType.Sound:
                Source.volume = 1f;
                break;
        }
    }

    private void GetAudioType(string name)
    {
        if (name.Contains("Background_"))
        {
            type = Constant.AudioType.Background;
        }
        else if(name.Contains("BackgroundEffect_"))
        {
            type = Constant.AudioType.BackgroundEffect;
        }
        else if (name.Contains("Sound_"))
        {
            type = Constant.AudioType.Sound;
        }
    }
}