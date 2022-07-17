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
                
                ToGainADiceRowOffset += 0.34f;
                break;
            
            case <= 4:
                if (worst) break;
                if (critical) ToGainADiceRowOffset += 0.5f;
                
                ToGainADiceRowOffset += 0.5f;
                break;
            
            case <= 9:
                ToGainADiceRowOffset += 0.75f;
                break;
            
            default:
                if (critical) ToGainADiceRowOffset += 1f;
                ToGainADiceRowOffset += 1f;
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
            case < 7:
                CastStandardCurse(rolledValue);
                break;
            default:
                CastHardCurse(rolledValue);
                break;
        }
    }
    
    private void CastEasyCurse(int rolledValue)
    {
        if (rolledValue <= 2)
        {
            EnemySpawnInterval = 0.25f;
            SpawningEnemiesCoroutine = StartCoroutine(SpawnEnemiesCoroutine(rolledValue));
        }
        else if (rolledValue <= 5)
        {
            RollType rollType = rolledValue == 5 ? RollType.Attack : RollType.Dodge;
            int value = rolledValue == 5 ? 2 : 1;
            
            LevelController.Instance.PlayerCurseDiceRoll(rollType, value);
        }
        else
        {
            LevelController.Instance.PlayerCurseDiceRoll(RollType.Life, 5);
        }
    }
    
    private void CastStandardCurse(int rolledValue)
    {
        if (rolledValue == 1) // 1
        {
            EnemySpawnInterval = 0.15f;
            SpawningEnemiesCoroutine = StartCoroutine(SpawnEnemiesCoroutine(2));
        }
        else if (rolledValue <= 2) // 2
        {
            CastStandardCurse(1);
            //todo Debuff de velocidade no player
        }
        else if (rolledValue <= 4) // 3 & 4
        {
            RollType rollType = rolledValue == 4 ? RollType.Magic : RollType.Attack;
            int value = rolledValue == 3 ? 2 : 3;
            
            LevelController.Instance.PlayerCurseDiceRoll(rollType, value);
        }
        else if (rolledValue == 5) // 5
        {
            CastEasyCurse(2);
            //Todo Spawnar fogueira num lugar aleatório do mapa?
        }
        else // 6
        {
            EnemySpawnInterval = 0.15f;
            SpawningEnemiesCoroutine = StartCoroutine(SpawnEnemiesCoroutine(3));
            //Todo buffar 3 inimgos com speed
            LevelController.Instance.PlayerCurseDiceRoll(RollType.Life, 10);
        }
    }

    private void CastHardCurse(int rolledValue)
    {
        //TODO BRINCAR AQUI
        CastStandardCurse(rolledValue == 6 ? 6 : 2);
        CastStandardCurse(4);
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