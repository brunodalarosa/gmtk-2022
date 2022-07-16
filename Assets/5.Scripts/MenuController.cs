using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
// ReSharper disable InconsistentNaming

namespace _5.Scripts
{
    public class MenuController : MonoBehaviour
    {
        public static void StartGame()
        {
            SceneManager.LoadScene("1.Scenes/Lipe");
        }

        public static void QuitGame()
        {
            Application.Quit();
        }
    }
}
