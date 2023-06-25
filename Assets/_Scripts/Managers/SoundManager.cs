using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityUtilities;

public class SoundManager : SingletonMonoBehaviour<SoundManager> 
{
    [SerializeField] 
    private AudioSource BGM_AudioSource;
    [SerializeField]
    private AudioSource SFX_AudioSource;
    
    public void PlaySound(AudioClip clip)
    {
        SFX_AudioSource.PlayOneShot(clip);
    }
    public void PlayBGM(AudioClip clip)
    {
        BGM_AudioSource.Stop();
        BGM_AudioSource.PlayOneShot(clip);
    }
}
