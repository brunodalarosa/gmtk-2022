using System;
using System.Collections;
using _5.Scripts.Gameplay;
using DG.Tweening;
using Global;
using UnityEngine;

namespace _5.Scripts
{
    public class EnemyStateMachine : MonoBehaviour
    {
        public enum State
        {
            Walking,
            Attacking,
            Dead
        }
        [SerializeField] private EnemyMovement _enemyMovement;
        [SerializeField] private EnemyData _enemyData;
        [SerializeField] private Damageable _damageable;
        [SerializeField] private Animator _animator;
        
        public State CurrentState { get; private set; }

        private void FixedUpdate()
        {
            if (_damageable.dead)
                ChangeState(State.Dead);
            
            switch (CurrentState)
            {
                case State.Walking:
                    var distanceToPlayer = Vector3.Distance(transform.position, _enemyMovement._playerTransform.position);
                    if (distanceToPlayer < _enemyData.AttackRange)
                        ChangeState(State.Attacking);
                    else
                    {
                        _animator.SetFloat("moveRatio", 1);
                        _enemyMovement.MoveTowardsPlayer(_enemyData.moveSpeed);
                    }
                    break;
                
                case State.Attacking:
                    break;
                
                case State.Dead:
                    StartCoroutine(DestroyEnemyCoroutine());
                    break;
                
                default:
                    break;
            }
        }

        private IEnumerator DestroyEnemyCoroutine()
        {
            yield return new WaitForSeconds(1);
            LevelController.Instance.RemoveAndDestroyEnemy(_enemyData);
        }

        private void ChangeState(State state)
        {
            switch (state)
            {
                case State.Walking:
                    _animator.SetFloat("moveRatio", 1);
                    break;
                case State.Attacking:
                    _animator.SetFloat("moveRatio", 0);
                    _animator.SetTrigger("attack");
                    break;
                case State.Dead:
                    break;
                default:
                    break;
            }
            CurrentState = state;
        }

        public void AttackEnd()
        {
            ChangeState(State.Walking);
        }
        
    }
}
