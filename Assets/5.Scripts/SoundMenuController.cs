using _5.Scripts;
using UnityEngine;
using UnityEngine.UI;

public class SoundMenuController : MonoBehaviour
{
    [SerializeField] private Slider BGMSlider;
    [SerializeField] private Slider SFXSlider;

    public void SetBGMVolume()
    {
        SoundManager.Instance.SetBGMVolume(BGMSlider.value);
    }

    public void SetSFXVolume()
    {
        SoundManager.Instance.SetSFXVolume(SFXSlider.value);
    }

}
