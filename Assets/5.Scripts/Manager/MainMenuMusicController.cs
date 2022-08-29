using Manager.Interface;
using UnityEngine;

namespace Manager
{
    public class MainMenuMusicController : MonoBehaviour
    {
        private void Start()
        { 
            Singletons.Instance.SoundManager.PlayBGM("bgm-menu"); 
        }
    }
}

