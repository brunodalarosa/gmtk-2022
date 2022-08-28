using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _5.Scripts
{
    public class HazardsSpawner : MonoBehaviour
    {
        [field:SerializeField] private List<GameObject> SpawnPoints { get; set; }
        
        [field:SerializeField] private GameObject FireHazardPrefab { get; set; }
        [field:SerializeField] private GameObject LightningPrefab { get; set; }
        [field:SerializeField] private GameObject GafanhotoPrefab { get; set; }
        [field:SerializeField] private GameObject DicePrefab { get; set; }

        private int _lastSpawnUsedIndex;
        
        private void Start()
        {
            _lastSpawnUsedIndex = -1;
        }

        public void SpawnHazards(HazardType hazardType, int quantity, float interval)
        {
            StartCoroutine(SpawnHazardsCoroutine(hazardType, quantity, interval));
        }

        private IEnumerator SpawnHazardsCoroutine(HazardType hazardType, int quantity, float interval)
        {
            var hazardPrefab = GetPrefabByType(hazardType);

            for (int i = 0; i < quantity; i++)
            {
                SpawnHazard(hazardPrefab);
                yield return new WaitForSeconds(interval);
            }
        }

        private GameObject GetPrefabByType(HazardType hazardType)
        {
            return hazardType switch
            {
                HazardType.Fire => FireHazardPrefab,
                HazardType.Lightning => LightningPrefab,
                HazardType.Gafanhoto => GafanhotoPrefab,
                _ => DicePrefab
            };
        }
        
        private void SpawnHazard(GameObject prefab)
        {
            string clipName = "";

            switch (Random.Range(0, 3))
            {
                case 0:
                    clipName = "miracle1";
                    break;
                case 1:
                    clipName = "miracle2";
                    break;
                case 2:
                    clipName = "miracle3";
                    break;

            }

            SoundManager.Instance?.PlaySFX(clipName);

            var selectedSpawnIndex = Random.Range(0, SpawnPoints.Count);
            
            // Help randomness
            if (selectedSpawnIndex == _lastSpawnUsedIndex)
                selectedSpawnIndex = selectedSpawnIndex > 0 ? selectedSpawnIndex - 1 : selectedSpawnIndex + 1;
            
            var spawnPoint = SpawnPoints[selectedSpawnIndex];

            Instantiate(prefab, spawnPoint.transform.position, Quaternion.identity);
        }

        private void OnDestroy()
        {
            StopAllCoroutines();
        }
    }

    public enum HazardType
    {
        Fire = 0,
        Lightning = 1,
        Gafanhoto = 2,
        Dice = 3
    }

}