using Global;
using UnityEngine;
using UnityEngine.UI;

public class DiceMenuController : MonoBehaviour
{
    [field: SerializeField] private DiceBagView DiceBagView { get; set; }
    
    [field: SerializeField] private Button RollAttackButton { get; set; }
    [field: SerializeField] private Button RollMagicButton { get; set; }
    [field: SerializeField] private Button RollDodgeButton { get; set; }
    [field: SerializeField] private Button RollLifeButton { get; set; }
    [field: SerializeField] private Button RollScoreButton { get; set; }
    [field: SerializeField] private Button ExitButton { get; set; }

    private void Awake()
    {
        RollAttackButton.onClick.AddListener(() => RollDice(RollType.Attack, DiceType.D6));
        RollMagicButton.onClick.AddListener(() => RollDice(RollType.Magic, DiceType.D6));
        RollDodgeButton.onClick.AddListener(() => RollDice(RollType.Dodge, DiceType.D6));
        RollLifeButton.onClick.AddListener(() => RollDice(RollType.Life, DiceType.D6));
        RollScoreButton.onClick.AddListener(() => RollDice(RollType.Score, DiceType.D6));
        
        ExitButton.onClick.AddListener(ExitDiceMenu);
    }

    public void Init(PlayerData playerData)
    {
        DiceBagView.Init(playerData.DiceBag);
    }

    private void UpdateButtons()
    {
        var playerDiceQtd = LevelController.instance.PlayerData.DiceQtd;

        ToggleButtonsEnabled(playerDiceQtd > 0);
    }

    private void RollDice(RollType rollType, DiceType diceType)
    {
        var usedDice = LevelController.instance.ApplyDiceRoll(rollType, diceType);

        if (usedDice != null)
        {
            DiceBagView.RemoveDice(usedDice.Type);
            UpdateButtons();
        }
    }
    
    private void ExitDiceMenu()
    {
        DiceBagView.Clear();
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
}
