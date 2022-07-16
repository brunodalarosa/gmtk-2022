using System.Collections;
using _5.Scripts;
using Global;
using UnityEngine;

public class Rngesus : MonoBehaviour
{
    [field: SerializeField] private EnemySpawner Spawner { get; set; }
    [field: SerializeField] private EnemyData EnemyPrefab { get; set; }

    [field: Header("Tweaking")]
    [field: SerializeField]
    private float Anger { get; set; } = 1f;

    [field: SerializeField] private int EnemySpawnPerLevel { get; set; } = 3;
    [field: SerializeField] private float DefaultSpawnInterval { get; set; } = 0.5f;

    public bool SpawningEnemies { get; set; }

    private Dice Dice { get; set; }
    
    private Coroutine SpawningEnemiesCoroutine { get; set; }

    private void Awake()
    {
        Dice = new Dice(DiceType.D6);
    }

    public void OnLevelStarted()
    {
        SpawningEnemiesCoroutine = StartCoroutine(SpawnEnemiesCoroutine(GetEnemySpawnQuantity()));
    }

    private int GetEnemySpawnQuantity()
    {
        //todo regra que leva em consideração Anger e randomness
        return EnemySpawnPerLevel;
    }

    private IEnumerator SpawnEnemiesCoroutine(int quantity)
    {
        SpawningEnemies = true;

        for (int i = 0; i < quantity; i++)
        {
            Spawner.SpawnEnemy(EnemyPrefab);
            yield return new WaitForSeconds(DefaultSpawnInterval);
        }

        SpawningEnemies = false;
    }

    public void OnLevelFinished()
    {
        Anger++;
    }

    private void OnDestroy()
    {
        StopCoroutine(SpawningEnemiesCoroutine);
    }
}