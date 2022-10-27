using UnityEngine;

public class Enemy : MonoBehaviour
{
    private const float Speed = 300;
    private const string Player = "Player";

    private Rigidbody _enemyRigidbody;
    private Transform _player;

    private void Awake()
    {
        _enemyRigidbody = GetComponent<Rigidbody>();
        _player = GameObject.FindWithTag(Player).transform;
    }

    private void Update()
    {
        Vector3 lookDirection = (_player.position - transform.position).normalized;
        _enemyRigidbody.AddForce(Speed * Time.deltaTime * lookDirection);
    }
}