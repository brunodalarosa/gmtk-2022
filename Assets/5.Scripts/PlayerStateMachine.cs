using Global;
using UnityEngine;
using UnityEngine.TextCore.Text;

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
                        if (_playerData.Attacks > 0)
                        {
                           ChangeState(State.Attacking);
                           return;
                        }
                    }
                    
                    if (Input.GetMouseButton(1))
                    {
                        if (_playerData.MagicShots > 0)
                        {
                            ChangeState(State.Casting);
                            return;
                        }
                    }

                    if (Input.GetKey(KeyCode.Space))
                    {
                        if (_playerData.Dodges > 0)
                        {
                            ChangeState(State.Dodging);
                            return;
                        }

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
                    _playerData.Attacks--;
                    LevelController.instance.UpdateUI();
                    break;
                case State.Dodging:
                    animator.SetTrigger("dodge");
                    _playerData.Dodges--;
                    LevelController.instance.UpdateUI();
                    break;
                case State.Casting:
                    animator.SetTrigger("cast");
                    _playerData.MagicShots--;
                    LevelController.instance.UpdateUI();
                    break;
                case State.Dying:
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

        public void DodgeEnd()
        {
            ChangeState(State.Walking);
        }
        
        public void CastEnd()
        {
            ChangeState(State.Walking);
        }
    }
}
