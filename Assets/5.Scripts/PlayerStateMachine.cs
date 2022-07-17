using _5.Scripts.Gameplay;
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
        private PlayerData _playerData;
        private Damageable damageable;
        private Animator animator;
        
        // Dodge
        private Vector3 currentDodgeDirection;
        private bool _dodgeMovementOccurring;

        public GameObject projectile;
        public Transform projectileOrigin;

        private void Awake()
        {
            _playerData = GetComponent<PlayerData>();
            animator = GetComponent<Animator>();
            damageable = GetComponent<Damageable>();
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
                    if (_dodgeMovementOccurring) 
                        _playerMovement.Dodge(currentDodgeDirection);
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
                    // Double check on dodging
                    damageable.dodging = false;
                    break;
                
                case State.Attacking:
                    _playerMovement.RotatePlayer();
                    SoundManager.Instance?.PlaySFX(Random.Range(0, 2) == 0 ? "swosh-01" : "swosh-02");
                    animator.SetTrigger("attack");
                    _playerData.Attacks--;
                    LevelController.Instance.UpdateUI();
                    break;
                
                case State.Dodging:
                    animator.SetTrigger("dodge");
                    SoundManager.Instance?.PlaySFX("swosh-03");
                    _playerData.Dodges--;
                    currentDodgeDirection = _playerMovement.GetDodgeDirection();
                    _dodgeMovementOccurring = true;
                    damageable.dodging = true;
                    LevelController.Instance.UpdateUI();
                    break;
                
                case State.Casting:
                    _playerMovement.RotatePlayer();
                    animator.SetTrigger("cast");
                    SoundManager.Instance?.PlaySFX("magic-missil");
                    _playerData.MagicShots--;
                    LevelController.Instance.UpdateUI();
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

        public void StopDodgeMovement()
        {
            _dodgeMovementOccurring = false;
            damageable.dodging = false;
        }
        public void CreateProjectile()
        {
            var proj = Instantiate(projectile, projectileOrigin.position, projectileOrigin.rotation);
            proj.transform.parent = null;
        }
    }
}
