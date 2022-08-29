using UnityEngine;

namespace Data
{
    public class Dice
    {
        public DiceType Type { get; }
        
        public int RolledValue { get; private set; }
        

        public Dice(DiceType type)
        {
            Type = type;
            RolledValue = 0;
        }

        public int Roll()
        {
            switch (Type)
            {
                case DiceType.D6:
                    var rng = Random.Range(1, 7);
                    RolledValue = rng;
                    return rng;
                
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