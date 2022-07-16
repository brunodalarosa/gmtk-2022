using System.Collections.Generic;
using Global;
using UnityEngine;

namespace _5.Scripts
{
    public class EnemySpawner : MonoBehaviour
    {
        [field:SerializeField] private List<GameObject> SpawnPoints { get; set; }

        public void SpawnEnemy(EnemyData enemyPrefab)
        {
            var spawnPoint = SpawnPoints[Random.Range(0, SpawnPoints.Count)];
            var enemy = Instantiate(enemyPrefab, spawnPoint.transform.position, Quaternion.identity);
            LevelController.instance.AddNewEnemy(enemy);
        }
        
    }
}
