using UnityEngine;

namespace _5.Scripts
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField] private CanvasRenderer pauseMenuPanel;

        private void Start()
        {
            if (SoundManager.Instance != null) 
                SoundManager.Instance?.PlayBGM("misforTune"); 
        }

        private void Update()
        {
            // Pause menu
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                OpenPauseMenu();
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
    }
}
