using System.Collections;
using System.Collections.Generic;
using Manager.Interface;
using UnityEngine;

namespace Controller
{
    public class Damageable : MonoBehaviour
    {
        [Header("Combat")]
        public int hpCurrent;
        [HideInInspector] public int hpMax;
        public float immunitySeconds;
        public List<int> attacksImmuneTo = new List<int>();
        public bool dead;
        public bool dodging;
        private Rigidbody rb;

        [Header("Components")]
        public VFXManager vfxManager;

        private void Awake()
        {
            hpMax = hpCurrent;
            vfxManager = GetComponent<VFXManager>();
            rb = GetComponent<Rigidbody>();
        }
        public void Damage(int value, int attackId)
        {
            if (!dead)
            {
                if (!dodging)
                {
                    if (attacksImmuneTo.Contains(attackId) == false)
                    {
                        if (gameObject.CompareTag("Player"))
                            Singletons.Instance.SoundManager?.PlaySFX(Random.Range(0, 2) == 0 ? "player-hurt-1" : "player-hurt-2");
                        else
                            Singletons.Instance.SoundManager?.PlaySFX(Random.Range(0, 2) == 0 ? "pierce-1" : "pierce-2");
                        
                        HpChange(-value);
                        StartCoroutine(Iframes(attackId));
                    }
                }
            }
        }

        public void Knockback(float knockbackForce, Vector3 knockbackSource)
        {
            Vector3 direction = transform.position - knockbackSource;
            if (rb != null)
                rb.AddForce(knockbackForce * direction);
               // rb.DOJump(transform.position + direction, .5f,1,.3f);
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

            if (CompareTag("Player"))
                Singletons.Instance.LevelManager.UpdatePlayerHp(value);

        }

        public void ApplyNewHpValue(int newHp)
        {
            hpCurrent = Mathf.Clamp(newHp, 0, hpMax);
        }

        public void Death()
        {
            dead = true;
            vfxManager?.VfxDeath();

            if (gameObject.CompareTag("Player"))
            {
                Singletons.Instance.SoundManager?.PlaySFX(Random.Range(0, 2) == 0 ? "player-die-1" : "player-die-2");
            }
            else // is enemy
            {
                Singletons.Instance.SoundManager?.PlaySFX(Random.Range(0, 3) == 0 ? "angel-die-1" : Random.Range(0, 2) == 0 ? "angel-die-2" : "angel-die-3");
            }
        }

    }
}
