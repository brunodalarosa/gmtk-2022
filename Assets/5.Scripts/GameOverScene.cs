using System;
using UnityEngine;
using UnityEngine.UI;

namespace _5.Scripts
{
    public class GameOverScene : MonoBehaviour
    {
        [field:SerializeField] private Button TryAgainButton { get; set; }

        private void Awake()
        {
            TryAgainButton.onClick.AddListener(ReturnToHome);
        }

        private void ReturnToHome()
        {
            SceneController.GoToMainMenu();
        }

        private void Start()
        {
            SoundManager.Instance?.PlaySFX("lose");
        }
    }
}