using UnityEngine;
using UnityEngine.UI;
// ReSharper disable InconsistentNaming

namespace _5.Scripts
{
    public class SoundMenuController : MonoBehaviour
    {
        [SerializeField] private Slider BGMSlider;
        [SerializeField] private Slider SFXSlider;

        public void Start()
        {
            if (SoundManager.Instance == null)
            {
                Debug.LogWarning("Missing SoundManager Instance. Start the game through the main menu.");
                return;
            }
            BGMSlider.value = SoundManager.Instance.BGMVolume;
            SoundManager.Instance.SetBGMVolume( SoundManager.Instance.BGMVolume);
            SFXSlider.value = SoundManager.Instance.SFXVolume;
            SoundManager.Instance.SetSFXVolume(SoundManager.Instance.SFXVolume);
        }

        public void SetBGMVolume()
        {
            if (SoundManager.Instance != null)  
                SoundManager.Instance.SetBGMVolume(BGMSlider.value);
        }

        public void SetSFXVolume()
        {
            if (SoundManager.Instance != null) 
                SoundManager.Instance.SetSFXVolume(SFXSlider.value);
        }

        public void PlaySFXUsingInstance(string sfx)
        {
            if (SoundManager.Instance != null) 
                SoundManager.Instance.PlaySFX(sfx);
        }

        public void PlayBGMUsingInstance(string bgm)
        {
            if (SoundManager.Instance != null) 
                SoundManager.Instance.PlayBGM(bgm);
        }

        public void StopBGMUsingInstance()
        {
            if (SoundManager.Instance != null) 
                SoundManager.Instance.StopAllBGM();
        }
    }
}
