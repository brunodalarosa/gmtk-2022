using UnityEngine;
using UnityEngine.SceneManagement;

namespace _5.Scripts
{
    public class SceneController : MonoBehaviour
    {
        public static void GoToGameScene()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene("1.Scenes/Lipe");
        }
    
        public static void GoToMainMenu()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene("Menu");
        }
    
        public static void QuitGame()
        {
            Time.timeScale = 1f;
            Application.Quit();
        }
    }
}
