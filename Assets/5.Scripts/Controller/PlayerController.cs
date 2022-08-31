using System;
using Data;
using Manager.Interface;
using UnityEngine;

namespace Controller
{
    public class PlayerController : MonoBehaviour
    {
        public PlayerData PlayerData { get; private set; }
        private DiceBag DiceBag { get; set; }

        public int DiceQtd => DiceBag.DiceQtd;

        [SerializeField] private DamageOnContact _playerAttackCollider;
        [SerializeField] private Damageable _damageable;

        private void Awake()
        {
            PlayerData = new PlayerData();
            DiceBag = new DiceBag();
            
            // Inventário inicial de dados do jogador
            DiceBag.AddNewDice(DiceType.D6);
            PlayerData.SetDiceQtd(DiceQtd);
            
            // Set the player damage
            _playerAttackCollider.SetDamage(PlayerData.AttackDamage);
        }

        public void AddDice(DiceType diceType)
        {
            DiceBag.AddNewDice(diceType);
            PlayerData.SetDiceQtd(DiceQtd);
        }
        
        public Dice GetDiceOfType(DiceType diceType)
        {
            return DiceBag.GetDiceOfType(diceType);
        }

        public void ApplyDiceRoll(RollType rollType, int rollValue)
        {
            switch (rollType)
            {
                case RollType.Life:
                    Singletons.Instance.SoundManager?.PlaySFX("heal");
                    RollHp(rollValue);
                    break;
                case RollType.Attack:
                    Singletons.Instance.SoundManager?.PlaySFX("attack-recharge");
                    RollAttack(rollValue);
                    break;
                case RollType.Magic:
                    Singletons.Instance.SoundManager?.PlaySFX("magic-refill");
                    RollMagic(rollValue);
                    break;
                case RollType.Dodge:
                    Singletons.Instance.SoundManager?.PlaySFX("dodge-recharge");
                    RollDodge(rollValue);
                    break;
                case RollType.Score:
                    Singletons.Instance.SoundManager?.PlaySFX("player-upgrade");
                    RollScore(rollValue);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(rollType), rollType, null);
            }
        }

        private void RollHp(int value)
        {
            PlayerData.Hp += value;

            PlayerData.Hp = Mathf.Clamp(PlayerData.Hp, 1.0f, 100.0f);

            _damageable.ApplyNewHpValue((int)PlayerData.Hp);
        }
        
        private void RollAttack(int value)
        {
            PlayerData.Attacks += value;
            if (PlayerData.Attacks < 0) PlayerData.Attacks = 0;
        }
        
        private void RollMagic(int value)
        {
            PlayerData.MagicShots += value;
            if (PlayerData.MagicShots < 0) PlayerData.MagicShots = 0;
        }
        
        private void RollDodge(int value)
        {
            PlayerData.Dodges += value;
            if (PlayerData.Dodges < 0) PlayerData.Dodges = 0;
        }
        
        private void RollScore(int value)
        {
            PlayerData.Score += value;
        }

        public void DiscardDice(Dice dice)
        {
            DiceBag.RemoveDice(dice);
            PlayerData.SetDiceQtd(DiceQtd);
        }

        public void AddScore(int enemyScore)
        {
            PlayerData.Score += enemyScore;
        }

        public void UpdateHp(int value)
        {
            PlayerData.Hp += value;
        }
    }
}