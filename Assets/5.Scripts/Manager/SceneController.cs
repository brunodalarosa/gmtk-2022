using Manager.Interface;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Manager
{
    public class SceneController : MonoBehaviour
    {
        public static void GoToGameScene()
        {
            Singletons.Instance.SoundManager.StopAllBGM();
            Time.timeScale = 1f;
            SceneManager.LoadScene("1.Scenes/Game");
        }
    
        public static void GoToMainMenu()
        {
            Singletons.Instance.SoundManager.StopAllBGM();
            Time.timeScale = 1f;
            SceneManager.LoadScene("1.Scenes/Menu");
        }
        
        public static void GoToGameOver()
        {
            Singletons.Instance.SoundManager.StopAllBGM();
            Time.timeScale = 1f;
            SceneManager.LoadScene("1.Scenes/GameOver");
        }

        public static void QuitGame()
        {
            Singletons.Instance.SoundManager.StopAllBGM();
            Time.timeScale = 1f;
            Application.Quit();
        }
    }
}
