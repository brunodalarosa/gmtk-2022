using Manager.Interface;
using UnityEngine;

namespace Controller
{
    public class PlaySfx : MonoBehaviour
    {
        public void PlayAudioString(string audio)
        {
            Singletons.Instance.SoundManager?.PlaySFX(audio);
        }
    }
}

