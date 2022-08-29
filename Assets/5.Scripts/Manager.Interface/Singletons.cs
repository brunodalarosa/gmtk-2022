using UnityEngine;

namespace Manager.Interface
{
    public class Singletons : MonoBehaviour
    {
        public static Singletons Instance { get; private set; }

        public ISoundManager SoundManager { get; private set; }
        public ILevelManager LevelManager { get; private set; }
        
        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public void Init(ISoundManager soundManager)
        {
            SoundManager = soundManager;
        }

        public void SetLevelManager(ILevelManager levelManager)
        {
            LevelManager = levelManager;
        }
    }
}