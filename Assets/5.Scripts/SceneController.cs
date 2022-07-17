using UnityEngine;
using UnityEngine.SceneManagement;

namespace _5.Scripts
{
    public class SceneController : MonoBehaviour
    {
        public static void GoToGameScene()
        {
            SoundManager.Instance?.StopAllBGM();
            Time.timeScale = 1f;
            SceneManager.LoadScene("1.Scenes/Game");
        }
    
        public static void GoToMainMenu()
        {
            SoundManager.Instance?.StopAllBGM();
            Time.timeScale = 1f;
            SceneManager.LoadScene("1.Scenes/Menu");
        }
        
        public static void GoToGameOver()
        {
            SoundManager.Instance?.StopAllBGM();
            Time.timeScale = 1f;
            SceneManager.LoadScene("1.Scenes/GameOver");
        }

        public static void QuitGame()
        {
            SoundManager.Instance?.StopAllBGM();
            Time.timeScale = 1f;
            Application.Quit();
        }
    }
}
