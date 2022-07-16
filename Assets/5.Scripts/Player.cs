using UnityEngine;

namespace Global
{
    public class Player 
    {
        [field:SerializeField] private int Hp { get; set; }
        [field:SerializeField] private int Attacks { get; set; }
        [field:SerializeField] private int Shots { get; set; }
        [field:SerializeField] private int Dodges { get; set; }
    }
}