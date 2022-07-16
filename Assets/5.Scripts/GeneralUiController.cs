using TMPro;
using UnityEngine;

namespace Global
{
    public class GeneralUiController : MonoBehaviour
    {
        [field: SerializeField] private TextMeshProUGUI HpLabelValue { get; set; }
        [field: SerializeField] private TextMeshProUGUI ScoreLabelValue { get; set; }
        [field: SerializeField] private TextMeshProUGUI AttackValue { get; set; }
        [field: SerializeField] private TextMeshProUGUI MagicValue { get; set; }
        [field: SerializeField] private TextMeshProUGUI DodgeValue { get; set; }

        public void UpdateHpValue(float hp)
        {
            HpLabelValue.text = $"HP:{(int)hp}";
        }

        public void UpdateScoreValue(int score)
        {
            ScoreLabelValue.text = $"Score:{score}";
        }

        public void UpdateAttackValue(int newAttackValue)
        {
            AttackValue.text = newAttackValue.ToString();
        }

        public void UpdateMagicValue(int newMagicValue)
        {
            MagicValue.text = newMagicValue.ToString();
        }

        public void UpdateDodgeValue(int newDodgeValue)
        {
            DodgeValue.text = newDodgeValue.ToString();
        }
    }
}
