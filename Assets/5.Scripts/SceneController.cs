using UnityEngine;
using UnityEngine.SceneManagement;

namespace _5.Scripts
{
    public class SceneController : MonoBehaviour
    {
        public static void GoToGameScene()
        {
            SceneManager.LoadScene("1.Scenes/Lipe");
        }
    
        public static void GoToMainMenu()
        {
            SceneManager.LoadScene("Menu");
        }
    
        public static void QuitGame()
        {
            Application.Quit();
        }
    }
}
