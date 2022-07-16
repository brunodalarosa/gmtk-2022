using UnityEngine;
using UnityEngine.UI;

namespace Global
{
    public class DiceView : MonoBehaviour
    {

        [field:SerializeField] private Image DiceImage { get; set; }

        public Dice Dice;
        
        public void Init(Dice dice)
        {
            Dice = dice;
        }
    }
}