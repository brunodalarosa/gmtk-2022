using System;
using UnityEngine;

namespace Global
{
    public class PlayerData : MonoBehaviour
    {
        private float Hp { get; set; }
        private int Attacks { get; set; }
        private int MagicShots { get; set; }
        private int Dodges { get; set; }
        private int Score { get; set; }

        public DiceBag DiceBag { get; private set; }

        public int DiceQtd => DiceBag.DiceQtd;

        private void Awake()
        {
            DiceBag = new DiceBag();
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