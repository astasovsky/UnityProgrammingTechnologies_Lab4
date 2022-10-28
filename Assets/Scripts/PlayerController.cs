using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform focalPoint;
    [SerializeField] private GameObject powerupIndicator;
    [SerializeField] private RocketBehaviour rocketPrefab;

    private const float Speed = 500;
    private const float PowerupStrength = 15;
    private const float HangTime = 0.5f;
    private const float SmashSpeed = 5;
    private const float ExplosionForce = 100;
    private const float ExplosionRadius = 10;
    private const string Powerup = "Powerup";
    private const string Enemy = "Enemy";

    private Rigidbody _playerRigidbody;
    private PowerUpType _currentPowerUp = PowerUpType.None;
    private Coroutine _powerupCountdown;
    private bool _smashing = false;
    private float _floorY;

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

        if (_currentPowerUp == PowerUpType.Smash && Input.GetKeyDown(KeyCode.Space) && !_smashing)
        {
            _smashing = true;
            StartCoroutine(Smash());
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

    private IEnumerator Smash()
    {
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        //Store the y position before taking off
        _floorY = transform.position.y;
        //Calculate the amount of time we will go up
        float jumpTime = Time.time + HangTime;
        while (Time.time < jumpTime)
        {
            //move the player up while still keeping their x velocity.
            _playerRigidbody.velocity = new Vector2(_playerRigidbody.velocity.x, SmashSpeed);
            yield return null;
        }

        //Now move the player down
        while (transform.position.y > _floorY)
        {
            _playerRigidbody.velocity = new Vector2(_playerRigidbody.velocity.x, -SmashSpeed * 2);
            yield return null;
        }

        //Cycle through all enemies.
        foreach (Enemy enemy in enemies)
        {
            //Apply an explosion force that originates from our position.
            if (enemy != null)
            {
                enemy.GetComponent<Rigidbody>().AddExplosionForce(ExplosionForce, transform.position, ExplosionRadius,
                    0.0f, ForceMode.Impulse);
            }
        }

        //We are no longer smashing, so set the boolean to false
        _smashing = false;
    }
}