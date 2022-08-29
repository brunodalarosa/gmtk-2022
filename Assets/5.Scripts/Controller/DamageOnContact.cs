using UnityEngine;

namespace Controller
{
    public class DamageOnContact : MonoBehaviour
    {
        [Header("Damage")]
        public int damage = 20;
        public bool randomizeId = true;
        public int id;

        [Header("Knockback")]
        [SerializeField] private float knockbackForce;
        private Transform knockBackSourcePosition;

        private void Awake()
        {
            if (randomizeId)
                id = Random.Range(0, 1000);

            if (GetComponentInParent<Damageable>())
                knockBackSourcePosition = GetComponentInParent<Damageable>().transform;
            else
                knockBackSourcePosition = transform;
        }

        public void SetDamage(int value)
        {
            damage = value;
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.GetComponentInChildren<Damageable>())
            {
                Damageable targetDamageable = other.gameObject.GetComponentInChildren<Damageable>();
                targetDamageable.Damage(damage, id);
                if(knockbackForce != 0)
                    targetDamageable.Knockback(knockbackForce, knockBackSourcePosition.position);

            }
        }
    }
}
