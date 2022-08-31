using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;
using Data;
using Manager.Interface;

namespace UI
{
    public class GeneralUiController : MonoBehaviour
    {
        [field: SerializeField] private TextMeshProUGUI HpLabelValue { get; set; }
        [field: SerializeField] private Image HpFillImage { get; set; }
        [field: SerializeField] private TextMeshProUGUI ScoreLabelValue { get; set; }
        [field: SerializeField] private TextMeshProUGUI AttackValue { get; set; }
        [field: SerializeField] private TextMeshProUGUI MagicValue { get; set; }
        [field: SerializeField] private TextMeshProUGUI DodgeValue { get; set; }
        [field: SerializeField] private TextMeshProUGUI[] DiceValue { get; set; }
        [field: SerializeField] private Animator AnimatorHp { get; set; }
        [field: SerializeField] private Animator AnimatorScore { get; set; }
        [field: SerializeField] private Animator AnimatorTimer { get; set; }
        [field: SerializeField] private Animator AnimatorAttackCounter { get; set; }
        [field: SerializeField] private Animator AnimatorSpellsCounter { get; set; }
        [field: SerializeField] private Animator AnimatorDodgesCounter { get; set; }
        [field: SerializeField] private Animator[] AnimatorDiceCounters { get; set; }
        [field: SerializeField] private Animator AnimatorDiceRoller { get; set; }

        [field: Header("Timer")]
        [field: SerializeField] private CanvasGroup TimerGroup { get; set; }
        [field: SerializeField] private TextMeshProUGUI TimerLabelValue { get; set; }
        [field: SerializeField] private AnimationCurve TimerCurve { get; set; }
        [field: SerializeField] private RectTransform GodDiceTransform { get; set; }
        [field: SerializeField] private Tween GodDiceTween { get; set; }
        [field: SerializeField] private Color[] GodDiceColors { get; set; }

        private void Start()
        {
            TimerGroup.alpha = 0;
            GodDiceTransform.DOScale(0, 0);
        }

        public void Refresh(PlayerData playerData, UiElementType elementType, float value)
        {
            HpLabelValue.text = ((int)playerData.Hp).ToString();
            ScoreLabelValue.text = playerData.Score.ToString();
            AttackValue.text = playerData.Attacks.ToString();
            MagicValue.text = playerData.MagicShots.ToString();
            DodgeValue.text = playerData.Dodges.ToString();
            DiceValue[0].text = playerData.DiceQtd.ToString();
            
            HpFillImage.DOFillAmount((float)playerData.Hp / (float)playerData.MaxHp, .25f);

            if (playerData.DiceQtd > 0)
                AnimatorDiceRoller.SetTrigger("normal");
            else
                AnimatorDiceRoller.SetTrigger("disable");

            if (elementType != UiElementType.none)
            {
                if (value >0)
                    AnimateElement(elementType, UiAnimationType.raise);
                else if (value <0)
                    AnimateElement(elementType, UiAnimationType.spend);
            }

            int threshold = 0;
            switch (elementType)
            {
                default:
                    break;
                case UiElementType.hp:
                    threshold = (int)(playerData.MaxHp * .33f);
                    UpdateCounterStatus(elementType, threshold, (int)playerData.Hp);
                    break;
                case UiElementType.counterAttack:
                    threshold = 5;
                    UpdateCounterStatus(elementType, threshold, playerData.Attacks);
                    break;
                case UiElementType.counterSpells:
                    threshold = 5;
                    UpdateCounterStatus(elementType, threshold, playerData.MagicShots);
                    break;
                case UiElementType.counterDodge:
                    threshold = 5;
                    UpdateCounterStatus(elementType, threshold, playerData.Dodges);
                    break;
                case UiElementType.counterD6:
                    threshold = 3;
                    UpdateCounterStatus(elementType, threshold, playerData.DiceQtd);
                    break;
            }
        }
        private void UpdateCounterStatus(UiElementType elementType, int threshold, int value)
        {
            if (value == 0)
                AnimateElement(elementType, UiAnimationType.qtyZero);
            else if (value <= threshold)
                AnimateElement(elementType, UiAnimationType.qtyLow);
            else
                AnimateElement(elementType, UiAnimationType.qtyNormal);
        }

        public void RollDice()
        {
            AnimatorDiceRoller.SetTrigger("press");
            Singletons.Instance.SoundManager.PlaySFX(Random.Range(0, 2) == 0 ? "dice-1" : "dice-2");
        }
        
        public void RollGodDice(int rolledValue)
        {
            Singletons.Instance.SoundManager.PlaySFX(Random.Range(0, 2) == 0 ? "dice-1" : "dice-2");

            GodDiceTransform.GetComponentInChildren<TextMeshProUGUI>().text = rolledValue.ToString();
            GodDiceTransform.GetComponentInChildren<TextMeshProUGUI>().color = GodDiceColors[rolledValue-1];

            Sequence seq = DOTween.Sequence();
            seq.Insert(0, GodDiceTransform.DOScale(1, .25f).SetEase(Ease.OutBack));
            seq.Insert(4, GodDiceTransform.DOScale(0, .25f).SetEase(Ease.OutBack));
            seq.Play();

        }

        public void SetTimer(float time)
        {
            StartCoroutine(TimerLabelUpdate(time));
            UpdateTimerStatus(UiAnimationType.qtyNormal);
            TimerGroup.DOFade(1, .5f).SetEase(TimerCurve);

        }
        public void EndTimer()
        {
            UpdateTimerStatus(UiAnimationType.qtyZero);
            TimerGroup.DOFade(0, .5f);
        }

        public void UpdateTimerStatus(UiAnimationType type)
        {
            AnimateElement(UiElementType.timer, type);
        }

        private IEnumerator TimerLabelUpdate(float time)
        {
            int count = (int)time;

            while (count > 0)
            {
                count--;
                TimerLabelValue.text = count.ToString();
                yield return new WaitForSeconds(1);
            }
        }


        public void AnimateElement(UiElementType elementType, UiAnimationType animType)
        {
            switch(elementType)
            {
                case UiElementType.hp:
                    Animate(AnimatorHp, animType);
                    break;
                case UiElementType.score:
                    Animate(AnimatorScore, animType);
                    break;
                case UiElementType.timer:
                    Animate(AnimatorTimer, animType);
                    break;
                case UiElementType.counterAttack:
                    Animate(AnimatorAttackCounter, animType);
                    break;
                case UiElementType.counterSpells:
                    Animate(AnimatorSpellsCounter, animType);
                    break;
                case UiElementType.counterDodge:
                    Animate(AnimatorDodgesCounter, animType);
                    break;
                case UiElementType.counterD6:
                    Animate(AnimatorDiceCounters[0], animType);
                    break;
            }
        }

        private void Animate(Animator anim, UiAnimationType animType)
        {
            switch(animType)
            {
                case UiAnimationType.raise:
                    anim.SetTrigger("gain");
                    break;
                case UiAnimationType.spend:
                    anim.SetTrigger("spend");
                    break;
                case UiAnimationType.noUse:
                    anim.SetTrigger("noUse");
                    break;
                case UiAnimationType.qtyLow:
                    anim.SetBool("low", true);
                    anim.SetBool("zero", false);
                    break;
                case UiAnimationType.qtyZero:
                    anim.SetBool("low", false);
                    anim.SetBool("zero", true);
                    break;
                case UiAnimationType.qtyNormal:
                    anim.SetBool("low", false);
                    anim.SetBool("zero", false);
                    break;
            }
        }


    }
}
