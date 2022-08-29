using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Controller
{
    public class Projectile : MonoBehaviour
    {
        public float speed;
        public Rigidbody body;
        public Animator animator;
        public float autoDestroyDelay = 3;

        private void Awake()
        {
            body = GetComponent<Rigidbody>();
            animator = GetComponent<Animator>();
            Invoke(nameof(Die), autoDestroyDelay);
            body.velocity = speed * transform.forward;

        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<Damageable>())
            {
                Blast();
            }
        }
        private void Blast()
        {
            animator.SetTrigger("blast");
            Destroy(gameObject, 5);
            body.velocity = Vector3.zero;
        }

        private void Die()
        {
            if(animator.GetCurrentAnimatorStateInfo(0).IsName("travel"))
            {
                Blast();

            }
        }

    }
}
