using System;
using System.Collections;
using System.Collections.Generic;
using _5.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using DG.Tweening;

namespace Global
{
    public class LevelController : MonoBehaviour
    {
        public static LevelController Instance;
        
        [field:Header("God Stuff")]
        [field:SerializeField] private Rngesus Rngesus { get; set; }

        [field: SerializeField] private float IntervalBetweenGodDiceRollTries { get; set; } = 5f;
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
        [field: SerializeField] private DiceMenuController RollDiceMenuOverlay { get; set; }
        
        [field: Header("Player")]
        [field: SerializeField] public PlayerData PlayerData { get; private set; }

        [field: Header("Filters")]
        public Volume filterBase;
        public Volume filterGod;
        public Volume filterPause;
        [field: SerializeField] private float PauseTimeScale { get; set; }
        [field: SerializeField] private bool paused { get; set; }
        
        private Coroutine GodDiceRollCoroutine { get; set; }

        private void Awake()
        {
            Instance = this;
            Enemies = new List<EnemyData>();
            
            DebugGetAd6Button.onClick.AddListener(DebugGiveD6ToPlayer);
        }

        public void UpdatePlayerHp(int value)
        {
            PlayerData.UpdateHp(value);
            UpdateUI();
        }

        private void Start()
        {
            GeneralUi.Refresh(PlayerData);
            UpdateDiceMenuButtonState(PlayerData);
            StartNewLevel();
        }

        public void StartNewLevel()
        {
            Rngesus.OnLevelStarted();
            GodDiceRollCoroutine = StartCoroutine(GodDiceRolling());
        }

        private IEnumerator GodDiceRolling()
        {
            yield return new WaitForSeconds(IntervalBetweenGodDiceRollTries);
            Rngesus.GodTryRoll();
        }

        public void AddNewEnemy(EnemyData enemy)
        {
            enemy.transform.SetParent(EnemiesParent.transform, true);
            Enemies.Add(enemy);
        }

        public void RemoveAndDestroyEnemy(EnemyData enemy)
        {
            if (Enemies.Remove(enemy))
            {
                PlayerData.AddScore(enemy.Score);
                GeneralUi.Refresh(PlayerData);
                
                Destroy(enemy.gameObject);
                CheckEnemies();
            }
            else
            {
                Debug.Log("ALGO MTO ESTRANHO ROLOU TENTOU REMOVER UM INIMIGO QUE NÃO ESTAVA NA LISTA");
            }
        }
        
        private void CheckEnemies()
        {
            if (Enemies.Count == 0 && !Rngesus.SpawningEnemies) FinishLevel();
        }
        
        private void FinishLevel()
        {
            StopCoroutine(GodDiceRollCoroutine);
            
            CurrentLevel++;
            Rngesus.OnLevelFinished();

            OnLevelCooldownCoroutine = StartCoroutine(LevelCooldownCoroutine());
        }

        private IEnumerator LevelCooldownCoroutine()
        {
            yield return new WaitForSeconds(BeforeEnterLevelCooldownSeconds);
            
            Debug.Log($"Entrou no level cooldown de {BetweenLevelsCooldownSeconds} segundos");
            //todo spawnar ferreiro?
            GeneralUi.SetTimer(BetweenLevelsCooldownSeconds);
            
            yield return new WaitForSeconds(BetweenLevelsCooldownSeconds *.66f);
            GeneralUi.UpdateTimerStatus(UiAnimationType.qtyLow);
            
            yield return new WaitForSeconds(BetweenLevelsCooldownSeconds *.33f);
            
            GeneralUi.EndTimer();
            StartNewLevel();
        }

        private void EnterDiceMenu()
        {
            paused = true;
            RollDiceMenuOverlay.InitAndRoll(PlayerData);
            Time.timeScale = PauseTimeScale;
            DOTween.defaultTimeScaleIndependent = true;
            DOTween.To(() => Time.timeScale, x => Time.timeScale= x, PauseTimeScale, .33f);
            DOTween.To(() => filterPause.weight, x => filterPause.weight = x, 1, .33f);
            DebugButtonParent.gameObject.SetActive(false);
            GeneralUi.transform.SetParent(RollDiceMenuOverlay.Content.transform, true);
            GeneralUi.RollDice();
        }

        public void LeaveDiceMenu()
        {
            paused = false;
            GeneralUi.transform.SetParent(Canvas.transform, true);
            DebugButtonParent.gameObject.SetActive(true);
            DOTween.To(() => Time.timeScale, x => Time.timeScale = x, 1, .33f);
            DOTween.To(() => filterPause.weight, x => filterPause.weight = x, 0, .33f);

            UpdateDiceMenuButtonState(PlayerData);
            Time.timeScale = 1;
        }

        public void ApplyDiceRoll(RollType rollType, int value)
        {
            Rngesus.OnPlayerRolledDice(value);
            PlayerData.ApplyDiceRoll(rollType, value);
            GeneralUi.Refresh(PlayerData);
        }

        public Dice GetDiceOfType(DiceType diceType)
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
            DiceQtdLabel.text = playerData.DiceQtd.ToString();
        }
        
        private void DebugGiveD6ToPlayer()
        {
            PlayerData.DiceBag.AddNewDice(DiceType.D6);
            UpdateDiceMenuButtonState(PlayerData);
        }

        public void UpdateUI()
        {
            GeneralUi.Refresh(PlayerData);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Tab) && PlayerData.DiceQtd > 0 && !paused)
            {
                EnterDiceMenu();

            }
        }
        
        public float AnimateGodDiceRoll(int rolledValue)
        {
            return 1.0f; //todo FARINHA aqui você chama todas as animações de deus rolando o dado com o valor que veio como parametro e depois retorna um float com os segundos que essas animações vão demorar! :D
        }

        public void PlayerCurseDiceRoll(RollType rollType, int value)
        {
            PlayerData.ApplyDiceRoll(rollType, value * -1);
            GeneralUi.Refresh(PlayerData);
        }

        private void OnDestroy()
        {
            StopAllCoroutines();
        }
    }
}
