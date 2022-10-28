using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private Enemy[] enemyPrefabs;
    [SerializeField] private PowerUp[] powerupPrefabs;
    [SerializeField] private Enemy bossPrefab;
    [SerializeField] private Enemy[] miniEnemyPrefabs;

    private const float SpawnRange = 9;
    private const int BossRound = 2;

    private int _enemiesCount;
    private int _waveNumber = 1;


    public void SpawnMiniEnemy(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            int randomMini = Random.Range(0, miniEnemyPrefabs.Length);
            _enemiesCount++;
            Enemy enemy = Instantiate(miniEnemyPrefabs[randomMini], GenerateSpawnPosition(),
                miniEnemyPrefabs[randomMini].transform.rotation);
            enemy.Destroying += OnDestroyingEnemy;
        }
    }

    private void Start()
    {
        SpawnEnemyWave(_waveNumber);
        SpawnRandomPowerUp();
    }

    private void SpawnEnemyWave(int enemiesToSpawn)
    {
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            int randomEnemy = Random.Range(0, enemyPrefabs.Length);
            Enemy enemy = Instantiate(enemyPrefabs[randomEnemy], GenerateSpawnPosition(),
                enemyPrefabs[randomEnemy].transform.rotation);
            _enemiesCount++;
            enemy.Destroying += OnDestroyingEnemy;
        }
    }

    private void SpawnRandomPowerUp()
    {
        int randomPowerup = Random.Range(0, powerupPrefabs.Length);
        Instantiate(powerupPrefabs[randomPowerup], GenerateSpawnPosition(),
            powerupPrefabs[randomPowerup].transform.rotation);
    }

    private void OnDestroyingEnemy()
    {
        _enemiesCount--;
        if (_enemiesCount == 0)
        {
            _waveNumber++;
            if (_waveNumber % BossRound == 0)
            {
                SpawnBossWave(_waveNumber);
            }
            else
            {
                SpawnEnemyWave(_waveNumber);
            }

            SpawnRandomPowerUp();
        }
    }

    private void SpawnBossWave(int currentRound)
    {
        int miniEnemiesToSpawn = currentRound / BossRound;
        _enemiesCount++;
        Enemy boss = Instantiate(bossPrefab, GenerateSpawnPosition(), bossPrefab.transform.rotation);
        boss.miniEnemySpawnCount = miniEnemiesToSpawn;
        boss.Destroying += OnDestroyingEnemy;
    }

    private static Vector3 GenerateSpawnPosition()
    {
        float spawnPosX = Random.Range(-SpawnRange, SpawnRange);
        float spawnPosZ = Random.Range(-SpawnRange, SpawnRange);
        Vector3 randomPos = new(spawnPosX, 0, spawnPosZ);
        return randomPos;
    }
}