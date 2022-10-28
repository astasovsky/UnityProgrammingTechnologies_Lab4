using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private const float Speed = 300;
    private const string Player = "Player";

    private Rigidbody _enemyRigidbody;
    private Transform _player;

    public event Action Destroying;

    private void Awake()
    {
        _enemyRigidbody = GetComponent<Rigidbody>();
        _player = GameObject.FindWithTag(Player).transform;
    }

    private void Update()
    {
        if (transform.position.y < -10)
        {
            Destroying?.Invoke();
            Destroy(gameObject);
        }

        Vector3 lookDirection = (_player.position - transform.position).normalized;
        _enemyRigidbody.AddForce(Speed * Time.deltaTime * lookDirection);
    }
}