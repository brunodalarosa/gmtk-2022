using System;
using UnityEngine;

namespace Global
{
    public class PlayerData : MonoBehaviour
    {
        public float MaxHp { get; private set; }
        public float Hp { get; private set; }
        public int Attacks { get; private set; }
        public int AttackDamage { get; private set; }
        public int MagicShots { get; private set; }
        public int Dodges { get; private set; }
        public int Score { get; private set; }

        public DiceBag DiceBag { get; private set; }

        public int DiceQtd => DiceBag.DiceQtd;

        private void Awake()
        {
            MaxHp = 100;
            Hp = 100;
            Attacks = 5;
            AttackDamage = 20;
            MagicShots = 3;
            Dodges = 5;
            Score = 0;
            
            DiceBag = new DiceBag();
            
            // Inventário inicial de dados do jogador
            DiceBag.AddNewDice(DiceType.D6);
        }

        public void RollDice(RollType rollType, Dice dice)
        {
            switch (rollType)
            {
                case RollType.Life:
                    RollHp(dice);
                    break;
                case RollType.Attack:
                    RollAttack(dice);
                    break;
                case RollType.Magic:
                    RollMagic(dice);
                    break;
                case RollType.Dodge:
                    RollDodge(dice);
                    break;
                case RollType.Score:
                    RollScore(dice);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(rollType), rollType, null);
            }
        }

        private void RollHp(Dice dice)
        {
            Hp += dice.Roll();
            DiscardDice(dice);
        }
        
        private void RollAttack(Dice dice)
        {
            Attacks += dice.Roll();
            DiscardDice(dice);
        }
        
        private void RollMagic(Dice dice)
        {
            MagicShots += dice.Roll();
            DiscardDice(dice);
        }
        
        private void RollDodge(Dice dice)
        {
            Dodges += dice.Roll();
            DiscardDice(dice);
        }
        
        private void RollScore(Dice dice)
        {
            Score += dice.Roll();
            DiscardDice(dice);
        }

        private void DiscardDice(Dice dice)
        {
            DiceBag.RemoveDice(dice);
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