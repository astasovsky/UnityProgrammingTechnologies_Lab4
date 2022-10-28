using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private Enemy enemyPrefab;
    [SerializeField] private GameObject powerupPrefab;

    private const float SpawnRange = 9;
    private int _enemiesCount;
    private int _waveNumber = 1;

    private void Start()
    {
        SpawnEnemyWave(_waveNumber);
        Instantiate(powerupPrefab, GenerateSpawnPosition(), powerupPrefab.transform.rotation);
    }

    private void SpawnEnemyWave(int enemiesToSpawn)
    {
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            Enemy enemy = Instantiate(enemyPrefab, GenerateSpawnPosition(), enemyPrefab.transform.rotation);
            _enemiesCount++;
            enemy.Destroying += OnDestroying;
        }
    }

    private void OnDestroying()
    {
        _enemiesCount--;
        if (_enemiesCount == 0)
        {
            _waveNumber++;
            SpawnEnemyWave(_waveNumber);
            Instantiate(powerupPrefab, GenerateSpawnPosition(), powerupPrefab.transform.rotation);
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