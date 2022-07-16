using System;
using System.Collections;
using System.Collections.Generic;
using _5.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Global
{
    public class LevelController : MonoBehaviour
    {
        public static LevelController instance;
        
        [field:Header("God Stuff")]
        [field:SerializeField] private Rngesus Rngesus { get; set; }
        [field:SerializeField] public GameObject EnemiesParent { get; set; }
        private List<EnemyData> Enemies { get; set; }


        [field: Header("Level Control")]
        [field: SerializeField] private int CurrentLevel { get; set; } = 0;
        [field:SerializeField] private float BeforeEnterLevelCooldownSeconds { get; set; } = 2;
        [field: SerializeField] private float BetweenLevelsCooldownSeconds { get; set; } = 10;
        
        private Coroutine OnLevelCooldownCoroutine { get; set; }

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
            

            
        }

        private void Start()
        {
            UpdateDiceMenuButtonState(PlayerData);
            StartNewLevel();
        }

        public void StartNewLevel()
        {
            Rngesus.OnLevelStarted();
        }

        public void AddNewEnemy(EnemyData enemy)
        {
            enemy.transform.SetParent(EnemiesParent.transform, true);
            Enemies.Add(enemy);
        }

        public void RemoveEnemy(EnemyData enemy)
        {
            if (Enemies.Remove(enemy)) CheckEnemies();
        }
        
        private void CheckEnemies()
        {
            if (Enemies.Count == 0 && !Rngesus.SpawningEnemies) FinishLevel();
        }
        
        private void FinishLevel()
        {
            CurrentLevel++;
            Rngesus.OnLevelFinished();

            OnLevelCooldownCoroutine = StartCoroutine(LevelCooldownCoroutine());
        }

        private IEnumerator LevelCooldownCoroutine()
        {
            yield return new WaitForSeconds(BeforeEnterLevelCooldownSeconds);
            //todo spawnar ferreiro?
            yield return new WaitForSeconds(BetweenLevelsCooldownSeconds);
            StartNewLevel();
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

        public void UpdateDiceMenuButtonState(PlayerData playerData)
        {
            RollDiceMenuButton.enabled = playerData.DiceQtd >= 1;
            DiceQtdLabel.text = playerData.DiceQtd.ToString();
        }
        
        private void DebugGiveD6ToPlayer()
        {
            PlayerData.DiceBag.AddNewDice(DiceType.D6);
            UpdateDiceMenuButtonState(PlayerData);
        }

        private void OnDestroy()
        {
            StopCoroutine(OnLevelCooldownCoroutine);
        }

        public void UpdateUI()
        {
            GeneralUi.Refresh(PlayerData);
        }
    }
}
