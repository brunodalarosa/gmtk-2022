using System.Collections;
using _5.Scripts;
using Global;
using UnityEngine;

public class Rngesus : MonoBehaviour
{
    [field: SerializeField] private EnemySpawner Spawner { get; set; }
    [field: SerializeField] private EnemyData EnemyPrefab { get; set; }
    
    [field: SerializeField] private HazardsSpawner HazardsSpawner { get; set; }

    [field: Header("Tweaking")]
    [field: SerializeField]
    private float Anger { get; set; } = 1f;

    [field: SerializeField] private int EnemySpawnPerLevel { get; set; } = 3;
    [field: SerializeField] private float DefaultSpawnInterval { get; set; } = 0.5f;
    
    private Dice Dice { get; set; }
    private Coroutine SpawningEnemiesCoroutine { get; set; }
    private Coroutine AnimatingGodCoroutine { get; set; }

    private float ToGainADiceRowOffset { get; set; } = 0;

    public bool SpawningEnemies { get; set; }
    public int AvailableDiceRolls { get; set; }
    public float EnemySpawnInterval { get; set; }

    private void Awake()
    {
        Dice = new Dice(DiceType.D6);
    }

    public void OnPlayerRolledDice(int rolledValue)
    {
        bool critical = rolledValue == 6;
        bool worst = rolledValue == 1;

        if (critical)
        {
            Anger += 0.1f;
        } 
        else if (worst)
        {
            Anger -= 0.1f;
        }
        
        switch (Anger)
        {
            case <= 2:
                if (worst) break;
                if (critical) ToGainADiceRowOffset += 0.5f;
                
                ToGainADiceRowOffset += 0.5f;
                break;
            
            case <= 4:
                if (worst) break;
                if (critical) ToGainADiceRowOffset += 0.5f;
                
                ToGainADiceRowOffset += 0.75f;
                break;
            
            case <= 9:
                ToGainADiceRowOffset += 1f;
                break;
            
            default:
                if (critical) ToGainADiceRowOffset += 1f;
                ToGainADiceRowOffset += 1.5f;
                break;
        }

        while (ToGainADiceRowOffset > 1.0f)
        {
            AvailableDiceRolls++;
            ToGainADiceRowOffset -= 1.0f;
        }
    }

    public void GodTryRoll()
    {
        if (!SpawningEnemies && AvailableDiceRolls > 0)
        {
            AvailableDiceRolls--;

            var rolledValue = Dice.Roll();
            
            Debug.Log($"ANGER {Anger} GOD DICE ROLL --> {rolledValue}");

            var godAnimSeconds = LevelController.Instance.AnimateGodDiceRoll(rolledValue);
            AnimatingGodCoroutine = StartCoroutine(WaitAnimationThenApplyDiceEffect(godAnimSeconds, rolledValue));
        }
        else
        {
            Debug.Log($"GOD DIDN'T ROLL, Offset = {ToGainADiceRowOffset}");
        }
    }

    private void CastGodCurse(int rolledValue)
    {
        switch (Anger)
        {
            case <= 3:
                CastEasyCurse(rolledValue);
                break;
            case <= 7:
                CastStandardCurse(rolledValue);
                break;
            default:
                CastHardCurse(rolledValue);
                break;
        }
    }
    
    private void CastEasyCurse(int rolledValue)
    {
        if (rolledValue == 1) // 1
        {
            var quantity = Dice.Roll();
            HazardsSpawner.SpawnHazards(HazardType.Dice, quantity > 4 ? quantity : quantity * 2, 0.5f);
        }
        else if (rolledValue <= 3) // 2 & 3
        {
            EnemySpawnInterval = 0.33f;
            SpawningEnemiesCoroutine = StartCoroutine(SpawnEnemiesCoroutine(rolledValue));
        }
        else if (rolledValue <= 5)
        {
            HazardsSpawner.SpawnHazards(HazardType.Lightning, 3, 0.33f);
        }
        else
        {
            CastEasyCurse(3); // spawn 3 inimigos
            HazardsSpawner.SpawnHazards(HazardType.Lightning, 2, 0.1f);
            HazardsSpawner.SpawnHazards(HazardType.Fire, 1, 1f);
        }
    }
    
    private void CastStandardCurse(int rolledValue) 
    {
        if (rolledValue == 1) // 1
        {
            HazardsSpawner.SpawnHazards(HazardType.Dice, Dice.Roll(), 0.5f);
        }
        else if (rolledValue <= 2) // 2
        {
            CastEasyCurse(3); // spawn 3 inimigos
        }
        else if (rolledValue <= 4) // 3 & 4
        {
            CastEasyCurse(3); // spawn 3 inimigos
            CastEasyCurse(5); // spawn 3 raios  
        }
        else if (rolledValue == 5) // 5
        {
            HazardsSpawner.SpawnHazards(HazardType.Fire, 2, 1f);
            EnemySpawnInterval = 0.25f;
            SpawningEnemiesCoroutine = StartCoroutine(SpawnEnemiesCoroutine(rolledValue));
        }
        else // 6
        {
            EnemySpawnInterval = 0.15f;
            SpawningEnemiesCoroutine = StartCoroutine(SpawnEnemiesCoroutine(3));
            
            HazardsSpawner.SpawnHazards(HazardType.Fire, 2, 0.75f);
            
            CastEasyCurse(5); // spawn 3 raios  
            
            HazardsSpawner.SpawnHazards(HazardType.Gafanhoto, 1, 1f);
        }
    }

    private void CastHardCurse(int rolledValue)
    {
        switch (rolledValue)
        {
            case 1:
                HazardsSpawner.SpawnHazards(HazardType.Dice, Dice.Roll(), 0.75f);
                break;
            
            case 2:
                CastStandardCurse(3); // Spawn 3 inimigos + 3 raios
                break;
            
            case 3:
                EnemySpawnInterval = 0.1f;
                SpawningEnemiesCoroutine = StartCoroutine(SpawnEnemiesCoroutine(4));
                HazardsSpawner.SpawnHazards(HazardType.Gafanhoto, 2, 0.5f);
                break;
            
            case 4:
                CastStandardCurse(5); // Spawn 5 inimigos + 2 fogos 
                HazardsSpawner.SpawnHazards(HazardType.Gafanhoto, 3, 1f);
                break;
            
            case 5: 
                CastEasyCurse(5); // spawn 3 raios
                CastStandardCurse(5); // Spawn 5 inimigos + 2 fogos 
                CastEasyCurse(5); // spawn 3 raios
                break;
            
            default:
                CastEasyCurse(5); // spawn 3 raios
                
                HazardsSpawner.SpawnHazards(HazardType.Fire, 3, 0.33f);
                
                EnemySpawnInterval = 0.1f;
                SpawningEnemiesCoroutine = StartCoroutine(SpawnEnemiesCoroutine(6));
                
                HazardsSpawner.SpawnHazards(HazardType.Gafanhoto, 2, 0.5f);
                break;
        }
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
        EnemySpawnInterval = DefaultSpawnInterval;
    }

    public void OnLevelFinished()
    {
        Anger++;

        if (Anger % 2 == 0) //Level par aumenta qtd de inimigos spawnados
        {
            EnemySpawnPerLevel += Dice.Roll() > 3 ? 2 : 1;
        }
    }
    
    private IEnumerator WaitAnimationThenApplyDiceEffect(float animDuration, int rolledValue)
    {
        yield return new WaitForSecondsRealtime(animDuration);
        CastGodCurse(rolledValue);
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}