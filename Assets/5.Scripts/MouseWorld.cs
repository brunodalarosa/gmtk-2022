using System;
using UnityEngine;

namespace _5.Scripts
{
    public class MouseWorld : MonoBehaviour
    {
        private static MouseWorld instance;
        private Transform playerTransform;

        [field: SerializeField] private LayerMask PlaneLayerMask;

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            playerTransform = FarinhaPlayerTest.instance.transform;
        }

        private void Update()
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition)))
            {
                transform.position = MouseWorld.GetPosition();
                RotatePlayer();
            }
        }

        private void RotatePlayer()
        {
            playerTransform.LookAt(new Vector3(transform.position.x, playerTransform.position.y, transform.position.z));

        }

        public static Vector3 GetPosition()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, instance.PlaneLayerMask);
            return raycastHit.point;
        }
    }
}