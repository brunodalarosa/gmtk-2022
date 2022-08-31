using System;
using Data;
using Manager.Interface;
using UnityEngine;

namespace Manager
{
    public class SoundManager : MonoBehaviour, ISoundManager
    {
        public Sound[] BGMs;
        public Sound[] SFXs;
        public float BGMVolume { get; private set; } = 100f;
        public float SFXVolume { get; private set; } = 100f;
        
        private AudioSource BGMAudioSource { get; set; }
        private AudioSource SfxAudioSource { get; set; }

        private void Awake()
        {
            BGMAudioSource = gameObject.AddComponent<AudioSource>();
            SfxAudioSource = gameObject.AddComponent<AudioSource>();
        }

        public void PlaySFX(string soundName)
        {
            var sound = Array.Find(SFXs, sound => sound.name == soundName);

            if (sound == null)
            {
                Debug.LogWarning("SFX: " + name + " not found!");
                return;
            }

            SfxAudioSource.PlayOneShot(sound.clip);
        }

        public void PlayBGM(string musicName)
        {
            var sound = Array.Find(BGMs, sound => sound.name == musicName);
            
            if (sound == null)
            {
                Debug.LogWarning("BGM: " + musicName + " not found!");
                return;
            }

            BGMAudioSource.clip = sound.clip;
            BGMAudioSource.Play();
        }
        
        public void StopBGM()
        {
            BGMAudioSource.Stop();
        }
        
        public void SetBGMVolume(float volume)
        {
            BGMVolume = volume;
            BGMAudioSource.volume = BGMVolume;
        }

        public void SetSFXVolume(float volume)
        {
            SFXVolume = volume;
            SfxAudioSource.volume = SFXVolume;
        }
    }
}
