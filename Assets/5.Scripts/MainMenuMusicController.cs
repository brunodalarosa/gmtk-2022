using System.Collections;
using System.Collections.Generic;
using _5.Scripts;
using UnityEngine;

public class MainMenuMusicController : MonoBehaviour
{
    private void Start()
    {
        if (SoundManager.Instance != null) 
            SoundManager.Instance?.PlayBGM("bgm-menu"); 
    }
}
