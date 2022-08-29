using Manager;
using Manager.Interface;
using UnityEngine;
using UnityEngine.SceneManagement;

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
            var soundManager = Instantiate(SoundManagerPrefab, Singletons.transform, false);

            Singletons.Init(soundManager);

            SceneManager.LoadScene("1.Scenes/Menu");
        }
    }
}