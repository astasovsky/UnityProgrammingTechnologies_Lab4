using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform focalPoint;
    [SerializeField] private GameObject powerupIndicator;
    [SerializeField] private RocketBehaviour rocketPrefab;

    private const float Speed = 500;
    private const float PowerupStrength = 15;
    private const string Powerup = "Powerup";
    private const string Enemy = "Enemy";

    private Rigidbody _playerRigidbody;
    private PowerUpType _currentPowerUp = PowerUpType.None;
    private Coroutine _powerupCountdown;

    private void Start()
    {
        _playerRigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        float forwardInput = Input.GetAxis("Vertical");
        _playerRigidbody.AddForce(Speed * forwardInput * Time.deltaTime * focalPoint.transform.forward);
        powerupIndicator.transform.position = transform.position + new Vector3(0, -0.5f, 0);

        if (_currentPowerUp == PowerUpType.Rockets && Input.GetKeyDown(KeyCode.F))
        {
            LaunchRockets();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Powerup))
        {
            Destroy(other.gameObject);
            _currentPowerUp = other.gameObject.GetComponent<PowerUp>().powerUpType;
            powerupIndicator.gameObject.SetActive(true);
            if (_powerupCountdown != null)
            {
                StopCoroutine(_powerupCountdown);
            }

            _powerupCountdown = StartCoroutine(PowerupCountdownRoutine());
        }
    }

    private IEnumerator PowerupCountdownRoutine()
    {
        yield return new WaitForSeconds(7);
        _currentPowerUp = PowerUpType.None;
        powerupIndicator.gameObject.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(Enemy) && _currentPowerUp == PowerUpType.Pushback)
        {
            Rigidbody enemyRigidbody = collision.gameObject.GetComponent<Rigidbody>();
            Vector3 awayFromPlayer = collision.gameObject.transform.position - transform.position;
            Debug.Log($"Player collided with: {collision.gameObject.name} with powerup set to {_currentPowerUp}");
            enemyRigidbody.AddForce(awayFromPlayer * PowerupStrength, ForceMode.Impulse);
        }
    }

    private void LaunchRockets()
    {
        foreach (Enemy enemy in FindObjectsOfType<Enemy>())
        {
            RocketBehaviour rocket = Instantiate(rocketPrefab, transform.position + Vector3.up, Quaternion.identity);
            rocket.Fire(enemy.transform);
        }
    }
}