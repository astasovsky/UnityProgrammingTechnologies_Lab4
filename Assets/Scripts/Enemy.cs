using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float speed = 300;
    [SerializeField] private bool isBoss;

    [HideInInspector] public int miniEnemySpawnCount;

    private const string Player = "Player";
    private const float SpawnInterval = 3;

    private Rigidbody _enemyRigidbody;
    private Transform _player;
    private float _nextSpawn;
    private SpawnManager _spawnManager;

    public event Action Destroying;

    private void Awake()
    {
        _enemyRigidbody = GetComponent<Rigidbody>();
        _player = GameObject.FindWithTag(Player).transform;
        if (isBoss)
        {
            _spawnManager = FindObjectOfType<SpawnManager>();
        }
    }

    private void Update()
    {
        if (transform.position.y < -10)
        {
            Destroying?.Invoke();
            Destroy(gameObject);
        }

        Vector3 lookDirection = (_player.position - transform.position).normalized;
        _enemyRigidbody.AddForce(speed * Time.deltaTime * lookDirection);

        if (isBoss && Time.time > _nextSpawn)
        {
            _nextSpawn = Time.time + SpawnInterval;
            _spawnManager.SpawnMiniEnemy(miniEnemySpawnCount);
        }
    }
}