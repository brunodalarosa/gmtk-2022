using UnityEngine;

namespace Global
{
    public class Dice : MonoBehaviour
    {
        public DiceType Type { get; private set; }

        public void Init(DiceType type)
        {
            Type = type;
        }

        public int Roll()
        {
            switch (Type)
            {
                case DiceType.D6:
                    return Random.Range(1, 6);
                
                default:
                    return 0;
            }
        }
    }

    public enum DiceType
    {
        D6 = 1
    }
}