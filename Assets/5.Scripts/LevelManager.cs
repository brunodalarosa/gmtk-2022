using System;
using UnityEngine;

namespace _5.Scripts
{
    public class LevelManager : MonoBehaviour
    {
        private void Start()
        {
            SoundManager.Instance.PlayBGM("sample-bgm"); 
        }
    }
}
