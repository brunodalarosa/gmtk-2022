using System;
using System.Collections.Generic;
using UnityEngine;

namespace Global
{
    public class DiceBag : MonoBehaviour
    {
        [field: SerializeField] private RectTransform ContentRect { get; set; }
        [field: SerializeField] private Dice DicePrefab { get; set; }

        private List<Dice> Dices { get; }

        public DiceBag()
        {
            Dices = new List<Dice>();
        }

        public void AddD6()
        {
            AddNewDice(DiceType.D6);
        }
        
        public void RemoveD6()
        {
            RemoveDice(DiceType.D6);
        }

        private void AddNewDice(DiceType diceType)
        {
            var newDice = Instantiate(DicePrefab, ContentRect, false);
            newDice.Init(diceType);
            Dices.Add(newDice);
        }

        private void RemoveDice(DiceType type)
        {
            var foundDice = Dices.Find(dice => dice.Type == type);

            if (foundDice == null)
            {
                throw new InvalidOperationException($"TENTANDO REMOVER {type} QUE NÃO EXISTE IIIH");
            }

            Dices.Remove(foundDice);
            Destroy(foundDice);
        }

    }
}