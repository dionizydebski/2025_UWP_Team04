using Singleton;
using UnityEngine;

namespace Core
{
    public class AudioManager : Singleton<AudioManager>
    {
        [Header("Audio Source")] 
        [SerializeField] public AudioSource musicSource;
        [SerializeField] AudioSource SFXSource;

        [Header("Audio Clip")] 
        public AudioClip background;
        public AudioClip arrow;
        public AudioClip arrowHit;
        public AudioClip damageToBase;

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
}
