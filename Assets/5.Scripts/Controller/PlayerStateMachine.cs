using Manager.Interface;
using UI;
using UnityEngine;

namespace Controller
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
        private PlayerController _playerController;
        private Damageable damageable;
        private Animator animator;
        
        // Dodge
        private Vector3 currentDodgeDirection;
        private bool _dodgeMovementOccurring;

        public GameObject projectile;
        public Transform projectileOrigin;

        private void Awake()
        {
            _playerController = GetComponent<PlayerController>();
            animator = GetComponent<Animator>();
            damageable = GetComponent<Damageable>();
        }

        public State CurrentState { get; private set; }

        private void Update()
        {
            switch (CurrentState)
            {
                case State.Walking:
                    if (Input.GetMouseButtonDown(0))
                    {
                        if (Singletons.Instance.LevelManager.GamePaused) return;
                        
                        if (_playerController.PlayerData.Attacks > 0)
                        {
                           ChangeState(State.Attacking);
                           return;
                        } 
                        Singletons.Instance.SoundManager?.PlaySFX("error");
                        Singletons.Instance.LevelManager.UiCounterNoUse(UiElementType.counterAttack);

                        return;
                    }
                    
                    if (Input.GetMouseButtonDown(1))
                    {
                        if (Singletons.Instance.LevelManager.GamePaused) return;
                        
                        if (_playerController.PlayerData.MagicShots > 0)
                        {
                            ChangeState(State.Casting);
                            return;
                        }
                        Singletons.Instance.SoundManager?.PlaySFX("error");
                        Singletons.Instance.LevelManager.UiCounterNoUse(UiElementType.counterSpells);
                        return;
                    }

                    if (Input.GetKeyDown(KeyCode.Space))
                    {
                        if (Singletons.Instance.LevelManager.GamePaused) return;
                        
                        if (_playerController.PlayerData.Dodges > 0)
                        {
                            ChangeState(State.Dodging);
                            return;
                        }
                        Singletons.Instance.SoundManager?.PlaySFX("error");
                        Singletons.Instance.LevelManager.UiCounterNoUse(UiElementType.counterDodge);
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
                    Singletons.Instance.SoundManager?.PlaySFX(Random.Range(0, 2) == 0 ? "swosh-01" : "swosh-02");
                    animator.SetTrigger("attack");
                    _playerController.PlayerData.Attacks--;
                    Singletons.Instance.LevelManager.UpdateUI(UiElementType.counterAttack, -1);
                    break;
                
                case State.Dodging:
                    animator.SetTrigger("dodge");
                    Singletons.Instance.SoundManager?.PlaySFX("dashing");
                    _playerController.PlayerData.Dodges--;
                    currentDodgeDirection = _playerMovement.GetDodgeDirection();
                    _dodgeMovementOccurring = true;
                    damageable.dodging = true;
                    Singletons.Instance.LevelManager.UpdateUI(UiElementType.counterDodge, -1);
                    break;
                
                case State.Casting:
                    _playerMovement.RotatePlayer();
                    animator.SetTrigger("cast");
                    Singletons.Instance.SoundManager?.PlaySFX("magic-missil");
                    _playerController.PlayerData.MagicShots--;
                    Singletons.Instance.LevelManager.UpdateUI(UiElementType.counterSpells, -1);
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
            Singletons.Instance.SoundManager?.PlaySFX("landing");
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
