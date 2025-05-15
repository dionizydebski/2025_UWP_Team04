using System;
using Singleton;
using UnityEngine;
using UnityEngine.Serialization;

public class AudioManager : Singleton<AudioManager>
{
    public static AudioManager audioManager;
    
    [Header("Audio Source")] 
    [SerializeField] public AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;

    [Header("Audio Clip")] 
    public AudioClip background;
    public AudioClip arrow;
    public AudioClip arrowHit;
    public AudioClip damageToBase;
    
    protected override void Awake()
    {
        base.Awake();
        audioManager = this;
    }

    private void Start()
    {
        musicSource.clip = background;
        musicSource.Play();
    }
    
    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }
}
