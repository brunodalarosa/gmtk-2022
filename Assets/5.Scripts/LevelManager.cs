using UnityEngine;

namespace _5.Scripts
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField] private CanvasRenderer pauseMenuPanel;
        [SerializeField] private CanvasRenderer gameOverPanel;

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
                OpenPauseMenu();
            }
            
            // DEBUG GAME OVER
            if (Input.GetKeyDown(KeyCode.G))
            {
                OpenGameOverMenu();
            }

        }

        private void OpenPauseMenu()
        {
            pauseMenuPanel.gameObject.SetActive(true);
            Time.timeScale = 0f;
        }

        public void ClosePauseMenu()
        {
            Time.timeScale = 1f;
            pauseMenuPanel.gameObject.SetActive(false);
        }

        public void OpenGameOverMenu()
        {
            SoundManager.Instance.PlaySFX("lose");
            gameOverPanel.gameObject.SetActive(true);
        }
    }
}
