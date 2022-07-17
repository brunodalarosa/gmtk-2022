using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropDice : MonoBehaviour
{
    [SerializeField] private Transform dicePrefab;
    [Range(1f, 100f)] public float dropChance;

    public void DropItem()
    {
        if (Random.Range(0f, 100f) < dropChance)
        {
            print("Drop!");
            Instantiate(dicePrefab, transform.position, Quaternion.identity);
        }
    }
}
