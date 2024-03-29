﻿using UnityEngine;

namespace Utils
{
    public class MouseWorld : MonoBehaviour
    {
        public static MouseWorld instance;

        [field: SerializeField] private LayerMask PlaneLayerMask;

        private void Awake()
        {
            instance = this;
        }

        private void Update()
        {
            transform.position = MouseWorld.GetPosition();
        }

        public static Vector3 GetPosition()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, instance.PlaneLayerMask);
            return raycastHit.point;
        }
    }
}