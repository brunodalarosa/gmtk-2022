using System;
using _5.Scripts;
using _5.Scripts.Gameplay;
using UnityEngine;

namespace Global
{
    public class PlayerData : MonoBehaviour
    {
        public float MaxHp { get; private set; }
        public float Hp { get; private set; }
        public int Attacks { get; set; }
        public int AttackDamage { get; private set; }
        public int MagicShots { get; set; }
        public int Dodges { get; set; }
        public int Score { get; private set; }

        public DiceBag DiceBag { get; private set; }

        public int DiceQtd => DiceBag.DiceQtd;

        [SerializeField] private DamageOnContact _playerAttackCollider;
        [SerializeField] private Damageable _damageable;
        

        private void Awake()
        {
            MaxHp = 100;
            Hp = 100;
            Attacks = 20;
            AttackDamage = 20;
            MagicShots = 10;
            Dodges = 10;
            Score = 0;
            
            DiceBag = new DiceBag();
            
            // Inventário inicial de dados do jogador
            DiceBag.AddNewDice(DiceType.D6);
            
            // Set the player damage
            _playerAttackCollider.SetDamage(AttackDamage);
        }

        public void ApplyDiceRoll(RollType rollType, int rollValue)
        {
            switch (rollType)
            {
                case RollType.Life:
                    SoundManager.Instance?.PlaySFX("heal");
                    RollHp(rollValue);
                    break;
                case RollType.Attack:
                    SoundManager.Instance?.PlaySFX("attack-recharge");
                    RollAttack(rollValue);
                    break;
                case RollType.Magic:
                    SoundManager.Instance?.PlaySFX("magic-refil");
                    RollMagic(rollValue);
                    break;
                case RollType.Dodge:
                    SoundManager.Instance?.PlaySFX("dodge-recharge");
                    RollDodge(rollValue);
                    break;
                case RollType.Score:
                    SoundManager.Instance?.PlaySFX("player-upgrade");
                    RollScore(rollValue);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(rollType), rollType, null);
            }
        }

        private void RollHp(int value)
        {
            Hp += value;

            if (Hp <= 0) Hp = 1;
            
            _damageable.ApplyNewHpValue((int)Hp);
        }
        
        private void RollAttack(int value)
        {
            Attacks += value;
            if (Attacks < 0) Attacks = 0;
        }
        
        private void RollMagic(int value)
        {
            MagicShots += value;
            if (MagicShots < 0) MagicShots = 0;
        }
        
        private void RollDodge(int value)
        {
            Dodges += value;
            if (Dodges < 0) Dodges = 0;
        }
        
        private void RollScore(int value)
        {
            Score += value;
        }

        public void DiscardDice(Dice dice)
        {
            DiceBag.RemoveDice(dice);
        }

        public void AddScore(int enemyScore)
        {
            Score += enemyScore;
        }

        public void UpdateHp(int value)
        {
            Hp += value;
        }
    }
    
    public enum RollType
    {
        Life = 1,
        Attack = 2,
        Magic = 3,
        Dodge = 4,
        Score = 5
    }
}