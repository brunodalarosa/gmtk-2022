using System;
using System.Collections.Generic;
using Data;
using UnityEngine;

namespace UI
{
    public class DiceBagView : MonoBehaviour
    {
        [field: SerializeField] private RectTransform ContentRect { get; set; }
        [field: SerializeField] private DiceView DiceViewPrefab { get; set; }

        private List<DiceView> DiceViews { get; set; }

        private void Awake()
        {
            DiceViews = new List<DiceView>();
        }

        public void Init(DiceBag diceBag)
        {
            foreach (var dice in diceBag.Dices)
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
            Destroy(foundDice.gameObject);
        }

        public void Clear()
        {
            foreach (var diceView in DiceViews)
            {
                Destroy(diceView.gameObject);
            }
            
            DiceViews.Clear();
        }

    }
}