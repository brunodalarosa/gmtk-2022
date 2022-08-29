using System.Collections.Generic;
using UnityEngine;

namespace Controller
{
    public class VFXManager : MonoBehaviour
    {
        public GameObject vfxStep;
        public ParticleSystem pfxDamage;
        public ParticleSystem pfxHeal;
        public ParticleSystem pfxDeath;
        public List<MeshRenderer> renderers = new List<MeshRenderer>();
    
        private void Awake()
        {
            renderers.AddRange(GetComponentsInChildren<MeshRenderer>());
        }
        public void VfxStep()
        {
            var vfx = Instantiate(vfxStep, transform.position, Quaternion.identity);
            vfx.transform.parent = null;
        }
        public void VfxDamage()
        {
            if (pfxDamage != null)
                pfxDamage.Play();
        }
        public void VfxHeal()
        {
            if (pfxHeal != null)
                pfxHeal.Play();
        }
        public void VfxDeath()
        {
            if(pfxDeath != null)
                pfxDeath.Play();
    
            if (GetComponent<Animator>())
                GetComponent<Animator>().SetTrigger("death");
    
        }
    
        public void DamageFlash(bool active)
        {
            int value = (active ? 1 : 0);
    
            foreach (var item in renderers)
            {
                item.material.SetInt("_Damaged", value);
            }
        }
    }
}

