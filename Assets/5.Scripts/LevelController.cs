using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Global
{
    public class LevelController : MonoBehaviour
    {
        public static LevelController instance;
        
        [field: Header("User Interface")]
        [field: SerializeField] private GameObject DebugButtonParent { get; set; }
        [field: SerializeField] private Button DebugGetAd6Button { get; set; }

        [field: SerializeField] private GeneralUiController GeneralUi { get; set; }

        [field: SerializeField] private TextMeshProUGUI DiceQtdLabel { get; set; }
        [field: SerializeField] private Button RollDiceMenuButton { get; set; }

        [field: SerializeField] private GameObject RollDiceMenuOverlay { get; set; }
        
        [field: Header("Player")]
        [field: SerializeField] public PlayerData PlayerData { get; private set; }

        private void Awake()
        {
            instance = this;
            
            DebugGetAd6Button.onClick.AddListener(DebugGiveD6ToPlayer);
        }

        public void ApplyDiceRoll(RollType rollType, DiceType diceType)
        {
            var dice = GetDiceOfType(diceType);
            
            if (dice != null) PlayerData.RollDice(rollType, dice);
        }

        private Dice GetDiceOfType(DiceType diceType)
        {
            try
            {
                return PlayerData.DiceBag.GetDiceOfType(diceType);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }
        
        private void DebugGiveD6ToPlayer()
        {
            PlayerData.DiceBag.AddNewDice(DiceType.D6);
        }
    }
}
