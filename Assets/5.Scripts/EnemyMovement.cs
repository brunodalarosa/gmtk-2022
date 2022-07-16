using UnityEngine;

namespace _5.Scripts
{
    public class EnemyMovement : MonoBehaviour
    {
        [SerializeField] private CharacterController _characterController;
        public Transform _playerTransform;
        
        private void Start()
        {
            _playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        }

        public void MoveTowardsPlayer(float moveSpeed)
        {
            transform.LookAt(_playerTransform);
            var playerPosition = _playerTransform.position;
            var myPosition = transform.position;
            var playerDirection = (playerPosition - myPosition).normalized;
            _characterController.Move(playerDirection * (moveSpeed * Time.deltaTime));
        }
    }
}
