using Manager.Interface;
using UnityEngine;

namespace Controller
{
    public class Collectable : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                GetComponent<Animator>().SetTrigger("get");
                Singletons.Instance.LevelManager.AddD6ToPlayer();
                Destroy(gameObject,5);
            }
        }
    }
}

