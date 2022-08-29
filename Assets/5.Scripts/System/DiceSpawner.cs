using UnityEngine;

namespace System
{
    public class DiceSpawner : MonoBehaviour
    {
        [SerializeField] private Transform[] spawnSlots;
        [SerializeField] private Transform dicePrefab;
        [Range(3f, 20f)] public float spawnCooldown;
        private float _currentTimer;

        private void Update()
        {
            _currentTimer += Time.deltaTime;
            if (_currentTimer < spawnCooldown) return;
            SpawnDice();
            _currentTimer = 0f;
        }

        private void SpawnDice()
        {
            var randomIndex = new Random().Next(0, spawnSlots.Length);
            Instantiate(dicePrefab, spawnSlots[randomIndex].transform.position, Quaternion.identity);
        }
    }
}
