using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarinhaPlayerTest : MonoBehaviour
{
    public Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
            animator.SetTrigger("attack");
        if (Input.GetKeyDown(KeyCode.Space))
            animator.SetTrigger("dodge");
        if (Input.GetMouseButtonDown(1))
            animator.SetTrigger("cast");
    }
}
