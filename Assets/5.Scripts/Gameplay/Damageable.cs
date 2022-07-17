using System.Collections;
using System.Collections.Generic;
using _5.Scripts;
using UnityEngine;

namespace Global
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
                if (!dodging)
                {
                    if (attacksImmuneTo.Contains(attackId) == false)
                    {
                        if (gameObject.CompareTag("Player"))
                            // NEED SOUND
                            SoundManager.Instance?.PlaySFX(Random.Range(0, 2) == 0 ? "player-die-1" : "player-die-2");
                        else
                            SoundManager.Instance?.PlaySFX(Random.Range(0, 2) == 0 ? "pierce-1" : "pierce-2");
                        
                        HpChange(-value);
                        StartCoroutine(Iframes(attackId));
                    }
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

            if (CompareTag("Player"))
                LevelController.instance.UpdatePlayerHp(value);

        }

        public void Death()
        {
            dead = true;
            vfxManager?.VfxDeath();

            if (gameObject.CompareTag("Player"))
                SoundManager.Instance?.PlaySFX(Random.Range(0, 2) == 0 ? "player-die-1" : "player-die-2");
            else
                SoundManager.Instance?.PlaySFX(Random.Range(0, 2) == 0 ? "angel-die-1" : "angel-die-2");
        }

    }
}
