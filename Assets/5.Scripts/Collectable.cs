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
            GetComponent<Animator>().SetTrigger("get");
            LevelController.Instance.AddD6ToPlayer();
            Destroy(gameObject,5);
        }
    }
}
