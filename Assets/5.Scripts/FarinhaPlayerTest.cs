using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarinhaPlayerTest : MonoBehaviour
{
    public float moveSpeed;
    public static FarinhaPlayerTest instance;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        movement.Normalize();

        if (!GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("player_attack"))
        {
            GetComponent<CharacterController>().Move(-movement * moveSpeed * Time.deltaTime);
        }

        if (movement != Vector3.zero)
            GetComponent<Animator>().SetFloat("moveRatio", 1);
        else
            GetComponent<Animator>().SetFloat("moveRatio", 0);

        if (Input.GetMouseButtonDown(0))
            GetComponent<Animator>().SetTrigger("attack");
        if (Input.GetMouseButtonDown(1))
            GetComponent<Animator>().SetTrigger("dodge");
    }
}
