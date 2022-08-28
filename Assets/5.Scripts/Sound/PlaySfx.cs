using System.Collections;
using System.Collections.Generic;
using _5.Scripts;
using UnityEngine;

public class PlaySfx : MonoBehaviour
{
    public void PlayAudioString(string audio)
    {
        SoundManager.Instance?.PlaySFX(audio);
    }
}
