using System;
using UnityEngine;

namespace Controller
{
    public class EnemyMovement : MonoBehaviour
    {
        [SerializeField] private Rigidbody _rigidBody;
        public Transform _playerTransform;
        //private Vector3 _enemyVelocity;
        
        private void Start()
        {
            _playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
            _rigidBody = GetComponent<Rigidbody>();
        }

        public void MoveTowardsPlayer(float moveSpeed)
        {
            transform.LookAt(_playerTransform);
            var playerDirection = (_playerTransform.position - transform.position).normalized;
            _rigidBody.position = Vector3.MoveTowards(transform.position, _playerTransform.position, moveSpeed*Time.deltaTime);
            //_characterController.Move(playerDirection * (moveSpeed * Time.deltaTime));

            
            // Gravity
            if (transform.position.y > 0)
            {
                _rigidBody.velocity += new Vector3(0,-9.8f * Time.deltaTime,0);
            }
            
        }
    }
}
