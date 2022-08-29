using Manager;
using Manager.Interface;
using UnityEngine;

namespace Bootstrapper
{
    public class Bootstrapper : MonoBehaviour
    {
        [field: SerializeField]
        private Singletons Singletons { get; set; }
        
        [field: SerializeField]
        private SoundManager SoundManagerPrefab { get; set; }

        private void Start()
        {
            var soundManager = Instantiate(SoundManagerPrefab, transform, false);

            Singletons.Init(soundManager);
        }
    }
}