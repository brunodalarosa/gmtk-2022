using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _5.Scripts.Gameplay
{
    public class Damageable : MonoBehaviour
    {
        [Header("Combat")]
        public int hpCurrent;
        [HideInInspector] public int hpMax;
        public float immunitySeconds;
        public List<int> attacksImmuneTo = new List<int>();
        public bool dead;

        [Header("Components")]
        public VFXManager vfxManager;

        private void Awake()
        {
            hpMax = hpCurrent;
            vfxManager = GetComponent<VFXManager>();
        }
        public void Damage(int value, int attackId)
        {
            if (!dead)
            {
                if (attacksImmuneTo.Contains(attackId) == false)
                {
                    HpChange(-value);
                    StartCoroutine(Iframes(attackId));
                }
            }
        }

        public IEnumerator Iframes(int id)
        {
            attacksImmuneTo.Add(id);
            vfxManager.DamageFlash(true);
            yield return new WaitForSeconds(immunitySeconds);
            vfxManager.DamageFlash(false);
            attacksImmuneTo.Remove(id);
        }

        public void HpChange(int value)
        {
            hpCurrent = Mathf.Clamp(hpCurrent + value, 0, hpMax);

            if (value < 0)
            {
                vfxManager?.VfxDamage();

                //PlayerMovement.Instance.Cam3();
            }
            else if (value > 0)
            {
                vfxManager?.VfxHeal();
            }

            if (hpCurrent <= 0)
                Death();

        }

        public void Death()
        {
            dead = true;
            vfxManager?.VfxDeath();
        }
    }
}
