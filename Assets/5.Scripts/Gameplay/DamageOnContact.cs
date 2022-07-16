using Global;
using UnityEngine;

namespace _5.Scripts.Gameplay
{
    public class DamageOnContact : MonoBehaviour
    {
        public int damage = 20;
        public bool randomizeId = true;
        public int id;

        private void Awake()
        {
            if (randomizeId)
                id = Random.Range(0, 1000);
        }

        public void SetDamage(int value)
        {
            damage = value;
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.GetComponentInChildren<Damageable>())
            {
                other.gameObject.GetComponentInChildren<Damageable>().Damage(damage, id);
            }
        }
    }
}
