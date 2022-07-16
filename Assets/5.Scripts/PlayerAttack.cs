using UnityEngine;

namespace _5.Scripts
{
    public class PlayerAttack : MonoBehaviour
    {
        [SerializeField] private PlayerStateMachine playerStateMachine; 
        
        public void AttackEnd()
        {
            playerStateMachine.ChangeState(PlayerStateMachine.State.Walking);
        }
    }
}
