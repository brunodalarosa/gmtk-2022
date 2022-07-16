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
        private Vector3 _playerVelocity;
        
        private float _playerAngle;
        private float _playerAngleSpeed = 1000; 
        
        void FixedUpdate()
        {
            Move();
            
            // Face direction
            _playerAngle += (Input.GetAxis("Mouse X") + Input.GetAxis("Mouse Y") / 2) * _playerAngleSpeed * -Time.deltaTime;
            _playerAngle = Mathf.Clamp(_playerAngle, 0, 360);
            transform.localRotation = Quaternion.AngleAxis(_playerAngle, Vector3.up);
         }

        private void Move()
        {
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");
            Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;
            
            if (direction.magnitude >= 0.1f)
            {
                controller.Move(direction * (moveSpeed * Time.deltaTime));
            }
            
            // Gravity
            _playerVelocity.y += -9.8f * Time.deltaTime;
            controller.Move(_playerVelocity * Time.deltaTime);
        }
    }
}
