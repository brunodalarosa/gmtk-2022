using System;
using UnityEngine;

namespace Data
{
    [Serializable]
    public class Sound
    {
        public string name;
        public AudioClip clip;

        [Range(0f, 1f)]
        public float volume;

        public bool loop;
    }
}