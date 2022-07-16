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
        
        [field: SerializeField] private Canvas Canvas { get; set; }
        [field: SerializeField] private GameObject DebugButtonParent { get; set; }
        [field: SerializeField] private Button DebugGetAd6Button { get; set; }

        [field: SerializeField] private GeneralUiController GeneralUi { get; set; }

        [field: SerializeField] private TextMeshProUGUI DiceQtdLabel { get; set; }
        [field: SerializeField] private Button RollDiceMenuButton { get; set; }

        [field: SerializeField] private DiceMenuController RollDiceMenuOverlay { get; set; }
        
        [field: Header("Player")]
        [field: SerializeField] public PlayerData PlayerData { get; private set; }

        private void Awake()
        {
            instance = this;
            
            DebugGetAd6Button.onClick.AddListener(DebugGiveD6ToPlayer);
            RollDiceMenuButton.onClick.AddListener(EnterDiceMenu);
            

            UpdateDiceMenuButtonState(PlayerData);
        }

        private void Start()
        {
            
            GeneralUi.Refresh(PlayerData);
        }

        private void EnterDiceMenu()
        {
            RollDiceMenuOverlay.Init(PlayerData);
            
            DebugButtonParent.gameObject.SetActive(false);
            GeneralUi.transform.SetParent(RollDiceMenuOverlay.Content.transform, true);
        }

        public void LeaveDiceMenu()
        {
            GeneralUi.transform.SetParent(Canvas.transform, true);
            DebugButtonParent.gameObject.SetActive(true);
            
            UpdateDiceMenuButtonState(PlayerData);
        }

        public Dice ApplyDiceRoll(RollType rollType, DiceType diceType)
        {
            var dice = GetDiceOfType(diceType);

            if (dice == null) return null;
            
            PlayerData.RollDice(rollType, dice);
            GeneralUi.Refresh(PlayerData);
            return dice;
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
            UpdateDiceMenuButtonState(PlayerData);
        }

        public void UpdateDiceMenuButtonState(PlayerData playerData)
        {
            RollDiceMenuButton.enabled = playerData.DiceQtd >= 1;
            DiceQtdLabel.text = playerData.DiceQtd.ToString();
        }

        public void UpdateUI()
        {
            GeneralUi.Refresh(PlayerData);
        }
    }
}