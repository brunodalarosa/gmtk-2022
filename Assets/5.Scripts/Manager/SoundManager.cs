using System;
using System.Linq;
using Data;
using Manager.Interface;
using UnityEngine;

// ReSharper disable InconsistentNaming

namespace Manager
{
    public class SoundManager : MonoBehaviour, ISoundManager
    {
        public Sound[] BGMs;
        public Sound[] SFXs;
        public float BGMVolume { get; private set; } = 100f;
        public float SFXVolume { get; private set; } = 100f;

        private void Awake()
        {
            //Todo: refatorar isso aqui para não ter um audioSource para audio do jogo
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
            BGMVolume = volume;
            foreach (var sound in BGMs)
            {
                sound.source.volume = volume;
            }
        }

        public void SetSFXVolume(float volume)
        {
            SFXVolume = volume;
            foreach (var sound in SFXs)
            {
                sound.source.volume = volume;
            }
        }
    }
}
