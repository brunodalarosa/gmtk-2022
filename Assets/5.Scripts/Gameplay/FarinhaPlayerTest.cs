using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarinhaPlayerTest : MonoBehaviour
{
    public float moveSpeed;
    public static FarinhaPlayerTest instance;
    public CharacterController characterController;
    public Animator animator;

    private void Awake()
    {
        instance = this;
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        movement.Normalize();

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("player_idle") || GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("player_move"))
        {
            characterController.Move(-movement * moveSpeed * Time.deltaTime);
        }

        if (movement != Vector3.zero)
            animator.SetFloat("moveRatio", 1);
        else
            animator.SetFloat("moveRatio", 0);

        if (Input.GetMouseButtonDown(0))
            animator.SetTrigger("attack");
        if (Input.GetMouseButtonDown(1))
            animator.SetTrigger("dodge");
        if (Input.GetKeyDown(KeyCode.E))
            animator.SetTrigger("cast");
    }
}
