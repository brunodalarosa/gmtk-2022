using UnityEngine;
using UnityEngine.TextCore.Text;

namespace _5.Scripts
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private CharacterController controller;
        [SerializeField] private float speed = 6f;
        // [SerializeField] private float turnSmoothTime = 0.1f;
        // [SerializeField] private float turnSmoothVelocity;

        void Update()
        {
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");
            Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;
            
            if (direction.magnitude >= 0.1f)
            {
                controller.Move(direction * (speed * Time.deltaTime));

                // float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
                // float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity,
                //     turnSmoothTime);
                // transform.rotation =
            }
            
        }
    }
}
