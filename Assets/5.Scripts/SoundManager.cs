using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

// ReSharper disable InconsistentNaming

namespace _5.Scripts
{
    public class SoundManager : MonoBehaviour
    {
        public Sound[] BGMs;
        public Sound[] SFXs;

        public static SoundManager Instance;

        private void Awake()
        { 
            // Singleton to carry volume changes
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);

            foreach (var sound in BGMs.Concat(SFXs))
            {
                sound.source = gameObject.AddComponent<AudioSource>();
                sound.source.clip = sound.clip;
                sound.source.volume = sound.volume;
                sound.source.loop = sound.loop;
            }
        }

        public void PlaySFX(string soundName)
        {
            var sound = Array.Find(SFXs, sound => sound.name == soundName);
            if (sound == null)
            {
                Debug.LogWarning("SFX: " + name + " not found!");
                return;
            }
            sound.source.PlayOneShot(sound.clip, 1f);
        }

        public void PlayBGM(string musicName)
        {
            var sound = Array.Find(BGMs, sound => sound.name == musicName);
            if (sound == null)
            {
                Debug.LogWarning("BGM: " + musicName + " not found!");
                return;
            }
            sound.source.Play();
        }
        
        public void StopAllBGM()
        {
            if (BGMs.Length < 1) return; 
            foreach (var bgm in BGMs)
            {
                bgm.source.Stop();
            }
        }
        
        public void SetBGMVolume(float volume)
        {
            foreach (var sound in BGMs)
            {
                sound.source.volume = volume;
            }
        }

        public void SetSFXVolume(float volume)
        {
            foreach (var sound in SFXs)
            {
                sound.source.volume = volume;
            }
        }
    }
}
