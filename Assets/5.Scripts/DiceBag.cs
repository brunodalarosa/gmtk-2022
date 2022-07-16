using System;
using System.Collections.Generic;

namespace Global
{
    public class DiceBag
    {
        public int DiceQtd => Dices.Count;

        public List<Dice> Dices { get; private set; }

        public DiceBag()
        {
            Dices = new List<Dice>();
        }

        public void AddNewDice(DiceType diceType)
        {
            var newDice = new Dice(diceType);
            Dices.Add(newDice);
        }
        
        public Dice GetDiceOfType(DiceType type)
        {
            var foundDice = Dices.Find(dice1 => dice1.Type == type);

            if (foundDice == null)
            {
                throw new InvalidOperationException($"TENTANDO REMOVER {type} QUE NÃO EXISTE IIIH");
            }

            return foundDice;
        }

        public void RemoveDice(Dice dice)
        {
            var foundDice = Dices.Find(dice1 => dice1 == dice);

            if (foundDice == null)
            {
                throw new InvalidOperationException($"TENTANDO REMOVER {dice.Type} QUE NÃO EXISTE IIIH");
            }

            Dices.Remove(foundDice);
        }
    }
}