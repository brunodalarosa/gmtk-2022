namespace Data
{
    public class PlayerData
    {
        public float MaxHp { get; set; }
        public float Hp { get; set; }
        public int Attacks { get; set; }
        public int AttackDamage { get; set; }
        public int MagicShots { get; set; }
        public int Dodges { get; set; }
        public int Score { get; set; }
        
        public int DiceQtd { get; set; }

        public PlayerData()
        {
            MaxHp = 100;
            Hp = 100;
            Attacks = 20;
            AttackDamage = 20;
            MagicShots = 10;
            Dodges = 10;
            Score = 0;
        }

        public void SetDiceQtd(int diceQtd)
        {
            DiceQtd = diceQtd;
        }
    }
}