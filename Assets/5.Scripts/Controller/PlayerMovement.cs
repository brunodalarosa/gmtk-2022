using UnityEngine;
using Utils;

namespace Controller
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private CharacterController controller;
        [SerializeField] private float moveSpeed = 6f;
        [SerializeField] private float dashSpeed = 2f;
        private Animator _animator;
        private Vector3 _playerVelocity;
        private float _playerAngle;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        public void HandleMovement()
        {
            if (_animator.GetCurrentAnimatorStateInfo(0).IsName("player_idle") || _animator.GetCurrentAnimatorStateInfo(0).IsName("player_move"))
            {
                Move();
                RotatePlayer();
            }

            // Gravity
            _playerVelocity.y += -9.8f * Time.deltaTime;
            controller.Move(_playerVelocity * Time.deltaTime);
        }

        private void Move()
        {
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");
            Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

            if (direction.magnitude >= 0.1f)
            {
                controller.Move(direction * (moveSpeed * Time.deltaTime));
                _animator.SetFloat("moveRatio", 1);
            }
            else
                _animator.SetFloat("moveRatio", 0);
        }

        public void RotatePlayer()
        {
            var CameraPosition = MouseWorld.GetPosition();
            transform.LookAt(new Vector3(CameraPosition.x, transform.position.y, CameraPosition.z));
        }

        public Vector3 GetDodgeDirection()
        {
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");
            return new Vector3(horizontal, 0f, vertical).normalized;
        }

        public void Dodge(Vector3 direction)
        {
            controller.Move(direction * (dashSpeed * Time.deltaTime));
        }
    }
}
