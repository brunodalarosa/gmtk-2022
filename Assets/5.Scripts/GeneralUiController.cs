using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;

namespace Global
{
    public enum UiAnimationType {raise, spend, noUse, qtyNormal, qtyLow, qtyZero}
    public enum UiElementType {hp,score, timer, counterAttack, counterSpells, counterDodge, counterD6}

    public class GeneralUiController : MonoBehaviour
    {
        [field: SerializeField] private TextMeshProUGUI HpLabelValue { get; set; }
        [field: SerializeField] private Image HpFillImage { get; set; }
        [field: SerializeField] private TextMeshProUGUI ScoreLabelValue { get; set; }
        [field: SerializeField] private TextMeshProUGUI AttackValue { get; set; }
        [field: SerializeField] private TextMeshProUGUI MagicValue { get; set; }
        [field: SerializeField] private TextMeshProUGUI DodgeValue { get; set; }
        [field: SerializeField] private Animator AnimatorHp { get; set; }
        [field: SerializeField] private Animator AnimatorScore { get; set; }
        [field: SerializeField] private Animator AnimatorTimer { get; set; }
        [field: SerializeField] private Animator AnimatorAttackCounter { get; set; }
        [field: SerializeField] private Animator AnimatorSpellsCounter { get; set; }
        [field: SerializeField] private Animator AnimatorDodgesCounter { get; set; }
        [field: SerializeField] private Animator[] AnimatorDiceCounters { get; set; }

        [field: Header("Timer")]
        [field: SerializeField] private CanvasGroup TimerGroup { get; set; }
        [field: SerializeField] private Image TimerFillImage { get; set; }
        [field: SerializeField] private TextMeshProUGUI TimerLabelValue { get; set; }
        [field: SerializeField] private AnimationCurve TimerCurve { get; set; }

        private void Start()
        {
            TimerGroup.alpha = 0;
            
        }

        public void Refresh(PlayerData playerData)
        {
            HpLabelValue.text = ((int)playerData.Hp).ToString();
            ScoreLabelValue.text = playerData.Score.ToString();
            AttackValue.text = playerData.Attacks.ToString();
            MagicValue.text = playerData.MagicShots.ToString();
            DodgeValue.text = playerData.Dodges.ToString();
            HpFillImage.DOFillAmount((float)playerData.Hp / (float)playerData.MaxHp, .25f);
        }

        public void UpdateStatus()
        {

        }

        public void SetTimer(float time)
        {
            StartCoroutine(TimerLabelUpdate(time));
            UpdateTimerStatus(UiAnimationType.qtyNormal);
            TimerGroup.DOFade(1, .5f).SetEase(TimerCurve);

            TimerFillImage.fillAmount = 1;
            TimerFillImage.DOFillAmount(0, time);

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
                    anim.SetTrigger("zero");
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
