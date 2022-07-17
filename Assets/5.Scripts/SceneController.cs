using UnityEngine;
using UnityEngine.SceneManagement;

namespace _5.Scripts
{
    public class SceneController : MonoBehaviour
    {
        public static void GoToGameScene()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene("1.Scenes/Game");
        }
    
        public static void GoToMainMenu()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene("1.Scenes/Menu");
        }
        
        public static void GoToGameOver()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene("1.Scenes/GameOver");
        }

        public static void QuitGame()
        {
            Time.timeScale = 1f;
            Application.Quit();
        }
    }
}
