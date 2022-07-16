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
        [SerializeField] private float speed = 6f;
        private float _playerAngle;
        private float _playerAngleSpeed = 1000; 
        
        void Update()
        {
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");
            Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

            if (direction.magnitude >= 0.1f)
            {
                controller.Move(direction * (speed * Time.deltaTime));
            }

            _playerAngle += (Input.GetAxis("Mouse X") + Input.GetAxis("Mouse Y") / 2) * _playerAngleSpeed * -Time.deltaTime;
            _playerAngle = Mathf.Clamp(_playerAngle, 0, 360);
            transform.localRotation = Quaternion.AngleAxis(_playerAngle, Vector3.up);
         }
    }
}
