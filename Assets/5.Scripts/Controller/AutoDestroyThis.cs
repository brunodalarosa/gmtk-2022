using System.Collections;
using UnityEngine;

namespace Controller
{
    public class AutoDestroyThis : MonoBehaviour
    {
        [field: SerializeField] private float SecondsToDestroy { get; set; } = 1f;

        private void Start()
        {
            StartCoroutine(DestroyCoroutine());
        }

        private IEnumerator DestroyCoroutine()
        {
            yield return new WaitForSeconds(SecondsToDestroy);
            Destroy(this.gameObject);
        }

        private void OnDestroy()
        {
            StopAllCoroutines();
        }
    }
}

