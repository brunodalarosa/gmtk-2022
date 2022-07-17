using System;
using System.Collections;
using Global;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class DiceMenuController : MonoBehaviour
{
    private static readonly int Active = Animator.StringToHash("active");
    private static readonly int Select = Animator.StringToHash("select");

    [field: SerializeField] public GameObject Content { get; set; }
    
    [field: SerializeField] private Button RollAttackButton { get; set; }
    [field: SerializeField] private Animator RollAttackAnimator { get; set; }
    [field: SerializeField] private Button RollMagicButton { get; set; }
    [field: SerializeField] private Animator RollMagicAnimator { get; set; }

    [field: SerializeField] private Button RollDodgeButton { get; set; }
    [field: SerializeField] private Animator RollDodgeAnimator { get; set; }

    [field: SerializeField] private Button RollLifeButton { get; set; }
    [field: SerializeField] private Animator RollLifeAnimator { get; set; }

    [field: SerializeField] private Button RollScoreButton { get; set; }
    [field: SerializeField] private Animator RollScoreAnimator { get; set; }

    [field: SerializeField] public GameObject DiceResultPanel { get; set; }
    [field: SerializeField] private TextMeshProUGUI DiceRollResult { get; set; }

    [field: Header("Dice Vfx")]
    [field: SerializeField] private ParticleSystem VfxRoll { get; set; }
    [field: SerializeField] private ParticleSystem VfxBad { get; set; }
    [field: SerializeField] private ParticleSystem VfxNeutral { get; set; }
    [field: SerializeField] private ParticleSystem VfxGood { get; set; }
    [field: SerializeField] private ParticleSystem VfxBest { get; set; }
    [field: SerializeField] private Color ColorBad { get; set; }
    [field: SerializeField] private Color ColorNeutral { get; set; }
    [field: SerializeField] private Color ColorGood { get; set; }
    [field: SerializeField] private Color ColorBest { get; set; }
    [field: SerializeField] private AnimationCurve ResultCurve { get; set; }
    
    private int RolledValue { get; set; }


    private void Awake()
    {
        RollAttackButton.onClick.AddListener(() => ApplyRolledDice(RollType.Attack, RolledValue));
        RollMagicButton.onClick.AddListener(() => ApplyRolledDice(RollType.Magic, RolledValue));
        RollDodgeButton.onClick.AddListener(() => ApplyRolledDice(RollType.Dodge, RolledValue));
        RollLifeButton.onClick.AddListener(() => ApplyRolledDice(RollType.Life, RolledValue));
        RollScoreButton.onClick.AddListener(() => ApplyRolledDice(RollType.Score, RolledValue));
                
        Content.gameObject.SetActive(false);
    }

    public void InitAndRoll(PlayerData playerData)
    {
        ToggleButtonsEnabled(false);
        
        Content.gameObject.SetActive(true);
        DiceResultPanel.gameObject.SetActive(false);

        var dice = LevelController.instance.GetDiceOfType(DiceType.D6);

        if (dice == null)
        {
            Debug.Log("ISSO NÃO DEVERIA ACONTECER NUNCA!");
            LevelController.instance.LeaveDiceMenu();
            return;
        }

        RolledValue = dice.Roll();
        playerData.DiscardDice(dice);

        StopAllCoroutines();
        StartCoroutine(RollSequence());
    }
    private IEnumerator RollSequence()
    {
        DiceResultPanel.gameObject.SetActive(true);
        DiceRollResult.text = RolledValue.ToString();
        DiceResultPanel.GetComponent<RectTransform>().DOScale(0, 0);

        VfxRoll.Play();
        yield return new WaitForSecondsRealtime(VfxRoll.main.duration);

        if (RolledValue == 6)
        {
            VfxBest.Play();
            DiceRollResult.color = ColorBest;
        }
        else if (RolledValue == 1)
        {
            VfxBad.Play();
            DiceRollResult.color = ColorBad;
        }
        else if (RolledValue == 5)
        {
            VfxGood.Play();
            DiceRollResult.color = ColorGood;
        }
        else
        {
            VfxNeutral.Play();
            DiceRollResult.color = ColorNeutral;
        }

        DiceResultPanel.GetComponent<RectTransform>().DOScale(1, .25f).SetEase(ResultCurve);


        //todo: farinha aqui chama a animação, rolledValue contém o valor rolado e chamar linha de baixo pós animação
        BeforeChoiceSetAnimatorStates();
        ToggleButtonsEnabled(true);

    }
    private void ApplyRolledDice(RollType rollType, int value)
    {
        AfterChoiceSetAnimatorStates(rollType);
        LevelController.instance.ApplyDiceRoll(rollType, value);
        DiceResultPanel.GetComponent<RectTransform>().DOScale(0, .25f).SetEase(ResultCurve);
        StopAllCoroutines();
        StartCoroutine(ExitDiceMenu());
        //todo: farinha aqui chama a animação, rolledValue contém o valor rolado e chamar linha de baixo pós animação
    }
    
    private IEnumerator ExitDiceMenu()
    {
        yield return new WaitForSecondsRealtime(.25f);
        DiceResultPanel.gameObject.SetActive(false);
        Content.gameObject.SetActive(false);
        LevelController.instance.LeaveDiceMenu();
    }
    
    private void ToggleButtonsEnabled(bool enableButton)
    {
        RollAttackButton.enabled = enableButton;
        RollMagicButton.enabled = enableButton;
        RollDodgeButton.enabled = enableButton;
        RollLifeButton.enabled = enableButton;
        RollScoreButton.enabled = enableButton;
    }

    private void BeforeChoiceSetAnimatorStates()
    {
        RollAttackAnimator.SetBool(Active, true);
        RollMagicAnimator.SetBool(Active, true);
        RollDodgeAnimator.SetBool(Active, true);
        RollLifeAnimator.SetBool(Active, true);
        RollScoreAnimator.SetBool(Active, true);
    }
    
    private void AfterChoiceSetAnimatorStates(RollType type)
    {
        RollAttackAnimator.SetBool(Active, false);
        RollMagicAnimator.SetBool(Active, false);
        RollDodgeAnimator.SetBool(Active, false);
        RollLifeAnimator.SetBool(Active, false);
        RollScoreAnimator.SetBool(Active, false);

        switch (type)
        {
            case RollType.Life:
                RollLifeAnimator.SetTrigger(Select);
                break;
            
            case RollType.Attack:
                RollAttackAnimator.SetTrigger(Select);
                break;
            
            case RollType.Magic:
                RollMagicAnimator.SetTrigger(Select);
                break;
            
            case RollType.Dodge:
                RollDodgeAnimator.SetTrigger(Select);
                break;
            
            case RollType.Score:
                RollScoreAnimator.SetTrigger(Select);
                break;
            
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }
    }
}
