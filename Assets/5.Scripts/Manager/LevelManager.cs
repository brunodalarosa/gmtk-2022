using System;
using System.Collections;
using System.Collections.Generic;
using Controller;
using Data;
using DG.Tweening;
using Manager.Interface;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace Manager
{
    public class LevelController : MonoBehaviour, ILevelManager
    {
        public static LevelController Instance;
        
        [field:Header("God Stuff")]
        [field:SerializeField] private Rngesus Rngesus { get; set; }

        [field: SerializeField] private float IntervalBetweenGodDiceRollTries { get; set; } = 3f;
        [field:SerializeField] public GameObject EnemiesParent { get; set; }
        [field: SerializeField] private Animator AnimatorRngesus { get; set; }

        private List<EnemyData> Enemies { get; set; }

        [field: Header("Level Control")]
        [field: SerializeField] private CanvasRenderer pauseMenuPanel;

        [field: SerializeField] private int CurrentLevel { get; set; } = 0;
        [field:SerializeField] private float BeforeEnterLevelCooldownSeconds { get; set; } = 2;
        [field: SerializeField] private float BetweenLevelsCooldownSeconds { get; set; } = 5;
        
        private Coroutine OnLevelCooldownCoroutine { get; set; }

        [field: Header("User Interface")]
        [field: SerializeField] private Canvas Canvas { get; set; }
        [field: SerializeField] private GameObject DebugButtonParent { get; set; }
        [field: SerializeField] private Button DebugGetAd6Button { get; set; }
        [field: SerializeField] private GeneralUiController GeneralUi { get; set; }
        [field: SerializeField] private TextMeshProUGUI DiceQtdLabel { get; set; }
        [field: SerializeField] private DiceMenuController RollDiceMenuOverlay { get; set; }
        
        [field: Header("Player")]
        [field: SerializeField] public PlayerController PlayerController { get; private set; }

        [field: Header("Filters")]
        public Volume filterBase;
        public Volume filterGod;
        public Volume filterPause;
        [field: SerializeField] private float PauseTimeScale { get; set; }
        [field: SerializeField] public bool GamePaused { get; set; }
        
        private Coroutine GodDiceRollCoroutine { get; set; }

        private void Awake()
        {
            Instance = this;
            Enemies = new List<EnemyData>();
            
            DebugGetAd6Button.onClick.AddListener(AddD6ToPlayer);
            Singletons.Instance.SetLevelManager(this);
        }

        public void UpdatePlayerHp(int value)
        {
            PlayerController.UpdateHp(value);
            UpdateUI(UiElementType.hp, value);

            if (PlayerController.PlayerData.Hp <= 0)
            {
                SceneController.GoToGameOver();
            }
        }

        private void Start()
        {
            Singletons.Instance.SoundManager?.PlayBGM("misforTune"); 
            DebugButtonParent.gameObject.SetActive(false);
            
            GeneralUi.Refresh(PlayerController.PlayerData, UiElementType.none, 0);
            UpdateDiceMenuButtonState();
            StartNewLevel();
        }

        public void StartNewLevel()
        {
            Rngesus.OnLevelStarted();
            GodDiceRollCoroutine = StartCoroutine(GodDiceRolling());
        }

        private IEnumerator GodDiceRolling()
        {
            while (true)
            {
                yield return new WaitForSeconds(IntervalBetweenGodDiceRollTries);
                Rngesus.GodTryRoll();
            }
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
                PlayerController.AddScore(enemy.Score);
                GeneralUi.Refresh(PlayerController.PlayerData, UiElementType.score,enemy.Score);
                
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
            GamePaused = true;
            RollDiceMenuOverlay.InitAndRoll(PlayerController, GetDiceOfType(DiceType.D6));
            Time.timeScale = PauseTimeScale;
            DOTween.defaultTimeScaleIndependent = true;
            DOTween.To(() => Time.timeScale, x => Time.timeScale= x, PauseTimeScale, .33f);
            DOTween.To(() => filterPause.weight, x => filterPause.weight = x, 1, .33f);
            // DebugButtonParent.gameObject.SetActive(false);
            GeneralUi.transform.SetParent(RollDiceMenuOverlay.Content.transform, true);
            GeneralUi.RollDice();

            UpdateDiceMenuButtonState();
            GeneralUi.Refresh(PlayerController.PlayerData, UiElementType.counterD6, -1);
        }

        public void LeaveDiceMenu()
        {
            GamePaused = false;
            GeneralUi.transform.SetParent(Canvas.transform, true);
            // DebugButtonParent.gameObject.SetActive(true);
            DOTween.To(() => Time.timeScale, x => Time.timeScale = x, 1, .33f);
            DOTween.To(() => filterPause.weight, x => filterPause.weight = x, 0, .33f);

            Time.timeScale = 1;
        }

        public void ApplyDiceRoll(RollType rollType, int value, int rolledValue)
        {
            Rngesus.OnPlayerRolledDice(rolledValue);
            PlayerController.ApplyDiceRoll(rollType, value);

            UiElementType element = UiElementType.score;

            switch(rollType)
            {
                case RollType.Life:
                    element = UiElementType.hp;
                    break;
                case RollType.Score:
                    element = UiElementType.score;
                    break;
                case RollType.Attack:
                    element = UiElementType.counterAttack;
                    break;
                case RollType.Magic:
                    element = UiElementType.counterSpells;
                    break;
                case RollType.Dodge:
                    element = UiElementType.counterDodge;
                    break;
            }

            GeneralUi.Refresh(PlayerController.PlayerData, element, value);
        }

        public Dice GetDiceOfType(DiceType diceType)
        {
            try
            {
                return PlayerController.DiceBag.GetDiceOfType(diceType);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        public void UpdateDiceMenuButtonState()
        {
            DiceQtdLabel.text = PlayerController.DiceQtd.ToString();
        }
        
        public void AddD6ToPlayer()
        {
            Singletons.Instance.SoundManager.PlaySFX("walk-1");
            PlayerController.DiceBag.AddNewDice(DiceType.D6);
            UpdateDiceMenuButtonState();
            GeneralUi.Refresh(PlayerController.PlayerData, UiElementType.counterD6, 1);
        }

        public void UpdateUI(UiElementType type, float value)
        {
            GeneralUi.Refresh(PlayerController.PlayerData, type, value);
        }
        public void UiCounterNoUse(UiElementType type)
        {
            GeneralUi.AnimateElement(type, UiAnimationType.noUse);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Tab) && !GamePaused)
            {
                if(PlayerController.DiceQtd > 0)
                    EnterDiceMenu();
                else
                {
                    Singletons.Instance.SoundManager.PlaySFX("error");
                    UiCounterNoUse(UiElementType.counterD6);
                }

                return;
            }
            
            if (Input.GetKeyDown(KeyCode.Escape) && !GamePaused)
            {
                OpenPauseMenu();
            }
        }
        
        private void OpenPauseMenu()
        {
            pauseMenuPanel.gameObject.SetActive(true);
            Time.timeScale = 0f;
        }

        public void ClosePauseMenu()
        {
            Time.timeScale = 1f;
            pauseMenuPanel.gameObject.SetActive(false);
        }
        
        public float AnimateGodDiceRoll(int rolledValue)
        {
            StartCoroutine(DiceRollSequence(rolledValue));
            return 5.0f;
        }

        private IEnumerator DiceRollSequence(int rolledValue)
        {
            AnimatorRngesus.SetBool("spawned", true);
            yield return new WaitForSeconds(2f);
            AnimatorRngesus.SetTrigger("dice");
            yield return new WaitForSeconds(2f);

            switch (rolledValue)
            {
                case 1:
                case 2:
                    AnimatorRngesus.SetTrigger("angry");
                    break;
                case 3:
                case 4:
                    break;
                case 5:
                case 6:
                    AnimatorRngesus.SetTrigger("happy");
                    break;

            }
            GeneralUi.RollGodDice(rolledValue);
            yield return new WaitForSeconds(1.5f);
            AnimatorRngesus.SetBool("spawned", false);
        }

        public void PlayerCurseDiceRoll(RollType rollType, int value)
        {
            PlayerController.ApplyDiceRoll(rollType, value * -1);

            UiElementType element = UiElementType.score;

            switch (rollType)
            {
                case RollType.Life:
                    element = UiElementType.hp;
                    break;
                case RollType.Score:
                    element = UiElementType.score;
                    break;
                case RollType.Attack:
                    element = UiElementType.counterAttack;
                    break;
                case RollType.Magic:
                    element = UiElementType.counterSpells;
                    break;
                case RollType.Dodge:
                    element = UiElementType.counterDodge;
                    break;
            }

            GeneralUi.Refresh(PlayerController.PlayerData, element, value);
        }

        private void OnDestroy()
        {
            StopAllCoroutines();
        }
    }
}
