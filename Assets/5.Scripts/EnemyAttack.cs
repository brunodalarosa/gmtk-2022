using UnityEngine;

namespace _5.Scripts
{
    public class EnemyAttack : MonoBehaviour
    {
        [SerializeField] private EnemyData enemyData;

        public void Attack()
        {
            print("Ataquei!");
        }

        public void EndAttack()
        {
            
        }
        
        // DEBUG ATTACK RANGE
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, enemyData.AttackRange);
        }
    }
}
