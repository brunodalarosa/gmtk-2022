using System;
using System.Collections;
using System.Collections.Generic;
using Global;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            LevelController.instance.AddD6ToPlayer();
            Destroy(gameObject);
        }
    }
}
