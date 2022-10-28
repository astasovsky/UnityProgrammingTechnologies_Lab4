using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private Enemy[] enemyPrefabs;
    [SerializeField] private PowerUp[] powerupPrefabs;

    private const float SpawnRange = 9;
    private int _enemiesCount;
    private int _waveNumber = 1;

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
            enemy.Destroying += OnDestroying;
        }
    }

    private void SpawnRandomPowerUp()
    {
        int randomPowerup = Random.Range(0, powerupPrefabs.Length);
        Instantiate(powerupPrefabs[randomPowerup], GenerateSpawnPosition(),
            powerupPrefabs[randomPowerup].transform.rotation);
    }

    private void OnDestroying()
    {
        _enemiesCount--;
        if (_enemiesCount == 0)
        {
            _waveNumber++;
            SpawnEnemyWave(_waveNumber);
            SpawnRandomPowerUp();
        }
    }

    private static Vector3 GenerateSpawnPosition()
    {
        float spawnPosX = Random.Range(-SpawnRange, SpawnRange);
        float spawnPosZ = Random.Range(-SpawnRange, SpawnRange);
        Vector3 randomPos = new(spawnPosX, 0, spawnPosZ);
        return randomPos;
    }
}