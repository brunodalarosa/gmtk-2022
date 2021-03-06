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

        private void Update()
        {
            switch (CurrentState)
            {
                case State.Walking:
                    if (Input.GetMouseButtonDown(0))
                    {
                        if (LevelController.Instance.GamePaused) return;
                        
                        if (_playerData.Attacks > 0)
                        {
                           ChangeState(State.Attacking);
                           return;
                        } 
                        SoundManager.Instance?.PlaySFX("error");
                        LevelController.Instance.UiCounterNoUse(UiElementType.counterAttack);

                        return;
                    }
                    
                    if (Input.GetMouseButtonDown(1))
                    {
                        if (LevelController.Instance.GamePaused) return;
                        
                        if (_playerData.MagicShots > 0)
                        {
                            ChangeState(State.Casting);
                            return;
                        }
                        SoundManager.Instance?.PlaySFX("error");
                        LevelController.Instance.UiCounterNoUse(UiElementType.counterSpells);
                        return;
                    }

                    if (Input.GetKeyDown(KeyCode.Space))
                    {
                        if (LevelController.Instance.GamePaused) return;
                        
                        if (_playerData.Dodges > 0)
                        {
                            ChangeState(State.Dodging);
                            return;
                        }
                        SoundManager.Instance?.PlaySFX("error");
                        LevelController.Instance.UiCounterNoUse(UiElementType.counterDodge);
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
                    SoundManager.Instance?.PlaySFX(Random.Range(0, 2) == 0 ? "swosh-01" : "swosh-02");
                    animator.SetTrigger("attack");
                    _playerData.Attacks--;
                    LevelController.Instance.UpdateUI(UiElementType.counterAttack, -1);
                    break;
                
                case State.Dodging:
                    animator.SetTrigger("dodge");
                    SoundManager.Instance?.PlaySFX("dashing");
                    _playerData.Dodges--;
                    currentDodgeDirection = _playerMovement.GetDodgeDirection();
                    _dodgeMovementOccurring = true;
                    damageable.dodging = true;
                    LevelController.Instance.UpdateUI(UiElementType.counterDodge, -1);
                    break;
                
                case State.Casting:
                    _playerMovement.RotatePlayer();
                    animator.SetTrigger("cast");
                    SoundManager.Instance?.PlaySFX("magic-missil");
                    _playerData.MagicShots--;
                    LevelController.Instance.UpdateUI(UiElementType.counterSpells, -1);
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
            SoundManager.Instance?.PlaySFX("landing");
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
