using UnityEngine;

namespace _5.Scripts
{
    public class EnemyStateMachine : MonoBehaviour
    {
        public enum State
        {
            Walking,
            Attacking,
            Dying
        }
        [SerializeField] private EnemyMovement _enemyMovement;
        [SerializeField] private EnemyAttack _enemyAttack;
        [SerializeField] private EnemyData _enemyData;
        
        public State CurrentState { get; private set; }

        private void FixedUpdate()
        {
            switch (CurrentState)
            {
                case State.Walking:
                    var distanceToPlayer = Vector3.Distance(transform.position, _enemyMovement._playerTransform.position);
                    if (distanceToPlayer < _enemyData.AttackRange)
                        ChangeState(State.Attacking);
                    else
                        _enemyMovement.MoveTowardsPlayer(_enemyData.moveSpeed);
                    break;
                
                case State.Attacking:
                    // Is attacking
                    
                    // DEBUG back to walking
                    var playerDistance = Vector3.Distance(transform.position, _enemyMovement._playerTransform.position);
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
                    _enemyAttack.Attack();
                    // Start attack
                    break;
                case State.Dying:
                    break;
                default:
                    break;
            }
            CurrentState = state;
        }
        
    }
}
