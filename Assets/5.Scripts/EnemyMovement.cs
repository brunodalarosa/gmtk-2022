using System;
using Global;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions.Must;

namespace _5.Scripts
{
    public class EnemyMovement : MonoBehaviour
    {
        public enum State
        {
            Walking,
            Attacking,
            Dying
        }

        [SerializeField] private CharacterController _characterController;
        [SerializeField] private EnemyData _enemyData;
        private Transform _playerTransform;  
        
        public State CurrentState { get; private set; }

        private void Start()
        {
            _playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        }
        
        private void FixedUpdate()
        {
            switch (CurrentState)
            {
                case State.Walking:
                    var distanceToPlayer = Vector3.Distance(transform.position, _playerTransform.position);
                    if (distanceToPlayer < _enemyData.AttackRange)
                        ChangeState(State.Attacking);
                    else
                        MoveTowardsPlayer();
                    break;
                
                case State.Attacking:
                    // Is attacking
                    
                    // DEBUG back to walking
                    var playerDistance = Vector3.Distance(transform.position, _playerTransform.position);
                    if (playerDistance > _enemyData.AttackRange)
                        ChangeState(State.Walking);
                    break;
                
                case State.Dying:
                    
                    break;
                
                default:
                    break;
            }
        }

        private void ChangeState(State state)
        {
            switch (state)
            {
                case State.Walking:
                    print("Back to walking!");
                    break;
                case State.Attacking:
                    print("Attack!");
                    // Start attack
                    break;
                case State.Dying:
                    break;
                default:
                    break;
            }
            CurrentState = state;
        }

        private void MoveTowardsPlayer()
        {
            transform.LookAt(_playerTransform);
            var playerPosition = _playerTransform.position;
            var myPosition = transform.position;
            var playerDirection = (playerPosition - myPosition).normalized;
            _characterController.Move(playerDirection * (_enemyData.moveSpeed * Time.deltaTime));
        }
        
        // DEBUG
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _enemyData.AttackRange);
        }
    }
}
