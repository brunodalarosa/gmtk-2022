using System;
using Global;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _5.Scripts
{
    public class GameOverScene : MonoBehaviour
    {
        [field:SerializeField] private Button TryAgainButton { get; set; }
        
        [field:SerializeField] private TextMeshProUGUI FinalScore { get; set; }

        private void Start()
        {
            TryAgainButton.onClick.AddListener(ReturnToHome);
            FinalScore.text = LevelController.Instance.PlayerData.Score.ToString();
            SoundManager.Instance?.PlaySFX("lose");
        }
        
        private void ReturnToHome()
        {
            SceneController.GoToMainMenu();
        }
    }
}