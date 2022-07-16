using System;
using Cinemachine;
using UnityEditor;
using UnityEngine;
using UnityEngine.TextCore.Text;

namespace _5.Scripts
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private CharacterController controller;
        [SerializeField] private float moveSpeed = 6f;
        [SerializeField] private Animator animator;
        private Vector3 _playerVelocity;
        
        private float _playerAngle;
        private float _playerAngleSpeed = 1000;

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }
        void FixedUpdate()
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("player_idle") || GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("player_move"))
            {
                Move();
                RotatePlayer();
            }

            // Gravity
            _playerVelocity.y += -9.8f * Time.deltaTime;
            controller.Move(_playerVelocity * Time.deltaTime);

        }

        private void Move()
        {
                float horizontal = Input.GetAxisRaw("Horizontal");
                float vertical = Input.GetAxisRaw("Vertical");
                Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

                if (direction.magnitude >= 0.1f)
                {
                    controller.Move(direction * (moveSpeed * Time.deltaTime));
                    animator.SetFloat("moveRatio", 1);
                }
                else
                    animator.SetFloat("moveRatio", 0);
        }

        private void RotatePlayer()
        {
            var CameraPosition = MouseWorld.GetPosition();

            transform.LookAt(new Vector3(CameraPosition.x, transform.position.y, CameraPosition.z));

        }
    }
}
