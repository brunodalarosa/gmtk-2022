using Global;
using UnityEngine;

namespace _5.Scripts.Gameplay
{
    public class DamageOnContact : MonoBehaviour
    {
        [SerializeField] private PlayerData _playerData;
        public bool randomizeId = true;
        public int id;

        private void Awake()
        {
            if (randomizeId)
                id = Random.Range(0, 1000);
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.GetComponentInChildren<Damageable>())
            {
                other.gameObject.GetComponentInChildren<Damageable>().Damage(_playerData.AttackDamage, id);
            }
        }
    }
}
