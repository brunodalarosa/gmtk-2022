using Manager.Interface;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Manager
{
    public class GameOverScene : MonoBehaviour
    {
        [field:SerializeField] private Button TryAgainButton { get; set; }
        
        [field:SerializeField] private TextMeshProUGUI FinalScore { get; set; }

        private void Start()
        {
            TryAgainButton.onClick.AddListener(ReturnToHome);
            FinalScore.text = LevelController.Instance.PlayerController.PlayerData.Score.ToString();
            Singletons.Instance.SoundManager.PlaySFX("lose");
        }
        
        private void ReturnToHome()
        {
            SceneController.GoToMainMenu();
        }
    }
}