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

        public void Refresh(PlayerData playerData)
        {
            HpLabelValue.text = $"HP:{(int)playerData.Hp}";
            ScoreLabelValue.text = $"Score:{playerData.Score}";
            AttackValue.text = playerData.Attacks.ToString();
            MagicValue.text = playerData.MagicShots.ToString();
            DodgeValue.text = playerData.Dodges.ToString();

        }
    }
}
