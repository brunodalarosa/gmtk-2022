using Manager.Interface;
using UnityEngine;
using UnityEngine.UI;
// ReSharper disable InconsistentNaming

namespace Manager
{
    public class SoundMenuController : MonoBehaviour
    {
        [SerializeField] private Slider BGMSlider;
        [SerializeField] private Slider SFXSlider;

        public void Start()
        {
            if (Singletons.Instance.SoundManager == null)
            {
                Debug.LogWarning("Missing SoundManager Instance. Start the game through the main menu.");
                return;
            }
            BGMSlider.value = Singletons.Instance.SoundManager.BGMVolume;
            Singletons.Instance.SoundManager.SetBGMVolume( Singletons.Instance.SoundManager.BGMVolume);
            SFXSlider.value = Singletons.Instance.SoundManager.SFXVolume;
            Singletons.Instance.SoundManager.SetSFXVolume(Singletons.Instance.SoundManager.SFXVolume);
        }

        public void SetBGMVolume()
        {
            if (Singletons.Instance.SoundManager != null)  
                Singletons.Instance.SoundManager.SetBGMVolume(BGMSlider.value);
        }

        public void SetSFXVolume()
        {
            if (Singletons.Instance.SoundManager != null) 
                Singletons.Instance.SoundManager.SetSFXVolume(SFXSlider.value);
        }

        public void PlaySFXUsingInstance(string sfx)
        {
            if (Singletons.Instance.SoundManager != null) 
                Singletons.Instance.SoundManager.PlaySFX(sfx);
        }

        public void PlayBGMUsingInstance(string bgm)
        {
            if (Singletons.Instance.SoundManager != null) 
                Singletons.Instance.SoundManager.PlayBGM(bgm);
        }

        public void StopBGMUsingInstance()
        {
            if (Singletons.Instance.SoundManager != null) 
                Singletons.Instance.SoundManager.StopBGM();
        }
    }
}
