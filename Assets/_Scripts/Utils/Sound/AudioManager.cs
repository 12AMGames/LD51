using UnityEngine.Audio;
using System;
using UnityEngine;
using System.Collections.Generic;

public static class AudioManager
{

    public static void PlaySound(SoundNames name)
    {
        GameObject soundObj = new GameObject("Sound");
        AudioSource soundSource = soundObj.AddComponent<AudioSource>();

        soundSource.PlayOneShot(GetSound(name), 1);
        soundObj.AddComponent<Despawn>();
        soundObj.GetComponent<Despawn>().timer = GetSound(name).length;
    }

    public static void PlaySoundAtPoint(SoundNames name, Vector3 position)
    {
        GameObject soundObj = new GameObject("Sound");
        AudioSource soundSource = soundObj.AddComponent<AudioSource>();
        soundSource.spatialBlend = 1;

        AudioSource.PlayClipAtPoint(GetSound(name), position);
        soundObj.AddComponent<Despawn>();
        soundObj.GetComponent<Despawn>().timer = GetSound(name).length;
    }

    static AudioClip GetSound(SoundNames sound)
    {
        AudioClip audioClip = null;
        foreach (Sound soundClip in GameManager.Instance.sounds)
        {
            if(sound == soundClip.name)
            {
                audioClip = soundClip.clip;
            }
        }
        return audioClip;
    }
}

public enum SoundNames 
{
    EnemyHurt,
    EnemyDie,
    PlayerGrunt,
    PlayerHurt
}

