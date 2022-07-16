using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOnContact : MonoBehaviour
{
    public int damage;
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
            other.gameObject.GetComponentInChildren<Damageable>().Damage(damage, id);
        }
    }
}
