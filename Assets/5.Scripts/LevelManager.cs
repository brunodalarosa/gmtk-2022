using UnityEngine;

namespace _5.Scripts
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField] private CanvasRenderer pauseMenuPanel;
        
        private void Start()
        {
            if (SoundManager.Instance != null) 
                SoundManager.Instance.PlayBGM("sample-bgm"); 
        }

        private void Update()
        {
            // Pause menu
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                pauseMenuPanel.gameObject.SetActive(true);
            }
        }
    }
}
