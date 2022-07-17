using System.Collections.Generic;
using Global;
using UnityEngine;

namespace _5.Scripts
{
    public class EnemySpawner : MonoBehaviour
    {
        [field:SerializeField] private List<GameObject> SpawnPoints { get; set; }
        private int lastSpawnUsedIndex;
        
        private void Start()
        {
            lastSpawnUsedIndex = -1;
        }
        
        public void SpawnEnemy(EnemyData enemyPrefab)
        {
            var selectedSpawnIndex = Random.Range(0, SpawnPoints.Count);
            // Help randomness
            if (selectedSpawnIndex == lastSpawnUsedIndex)
                selectedSpawnIndex = selectedSpawnIndex > 0 ? selectedSpawnIndex - 1 : selectedSpawnIndex + 1;
            var spawnPoint = SpawnPoints[selectedSpawnIndex];
            lastSpawnUsedIndex = selectedSpawnIndex;
            
            var enemy = Instantiate(enemyPrefab, spawnPoint.transform.position, Quaternion.identity);
            LevelController.Instance.AddNewEnemy(enemy);
        }
        
    }
}
