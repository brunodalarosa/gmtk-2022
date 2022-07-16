using Global;
using UnityEngine;

namespace _5.Scripts
{
    public class PlayerStateMachine : MonoBehaviour
    {
        public enum State
        {
            Walking,
            Attacking,
            Dodging,
            Casting,
            Dying
        }
        
        [SerializeField] private PlayerMovement _playerMovement;
        [SerializeField] private PlayerAttack _playerAttack;
        [SerializeField] private PlayerData _playerData;
        
        public Animator animator;

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        public State CurrentState { get; private set; }

        private void FixedUpdate()
        {
            switch (CurrentState)
            {
                case State.Walking:
                    if (Input.GetMouseButton(0))
                    {
                        ChangeState(State.Attacking);
                        return;
                    }
                    
                    if (Input.GetMouseButton(1))
                    {
                        ChangeState(State.Casting);
                        return;
                    }

                    if (Input.GetKeyDown(KeyCode.Space))
                    {
                        ChangeState(State.Dodging);
                        return;
                    }
                    
                    _playerMovement.HandleMovement();
                    break;
                
                case State.Attacking:
                    // Is attacking
                    
                    break;
                
                case State.Casting:
                    break;
                
                case State.Dodging:
                    break;
                
                case State.Dying:
                    break;

                default:
                    break;
            }
        }

        public void ChangeState(State state)
        {
            switch (state)
            {
                case State.Walking:
                    break;
                case State.Attacking:
                    animator.SetTrigger("attack");
                    break;
                case State.Dodging:
                    animator.SetTrigger("dodge");
                    break;
                case State.Casting:
                    animator.SetTrigger("cast");
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
