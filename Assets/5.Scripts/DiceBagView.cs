using System;
using System.Collections.Generic;
using UnityEngine;

namespace Global
{
    public class DiceBagView : MonoBehaviour
    {
        public static DiceBagView instance;
        
        [field: SerializeField] private RectTransform ContentRect { get; set; }
        [field: SerializeField] private DiceView DiceViewPrefab { get; set; }

        private List<DiceView> DiceViews { get; set; }

        private void Awake()
        {
            instance = this;
            DiceViews = new List<DiceView>();
        }

        public void Init(List<Dice> diceList)
        {
            foreach (var dice in diceList)
            {
                var diceView = Instantiate(DiceViewPrefab, ContentRect, false);
                diceView.Init(dice);
                DiceViews.Add(diceView);
            }
        }

        public void AddDice(Dice dice)
        {
            var diceView = Instantiate(DiceViewPrefab, ContentRect, false);
            diceView.Init(dice);
            DiceViews.Add(diceView);
        }

        public void RemoveDice(DiceType type)
        {
            var foundDice = DiceViews.Find(Dview => Dview.Dice.Type == type);

            if (foundDice == null)
            {
                throw new InvalidOperationException($"TENTANDO REMOVER {type} QUE NÃO EXISTE IIIH");
            }

            DiceViews.Remove(foundDice);
            Destroy(foundDice);
        }

    }
}