using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform focalPoint;
    [SerializeField] private GameObject powerupIndicator;

    private const float Speed = 500;
    private const float PowerupStrength = 15;
    private const string Powerup = "Powerup";
    private const string Enemy = "Enemy";
    private Rigidbody _playerRigidbody;
    private bool _hasPowerup;

    private void Start()
    {
        _playerRigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        float forwardInput = Input.GetAxis("Vertical");
        _playerRigidbody.AddForce(Speed * forwardInput * Time.deltaTime * focalPoint.transform.forward);
        powerupIndicator.transform.position = transform.position + new Vector3(0, -0.5f, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Powerup))
        {
            _hasPowerup = true;
            Destroy(other.gameObject);
            powerupIndicator.gameObject.SetActive(true);
            StartCoroutine(PowerupCountdownRoutine());
        }
    }

    private IEnumerator PowerupCountdownRoutine()
    {
        yield return new WaitForSeconds(7);
        _hasPowerup = false;
        powerupIndicator.gameObject.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(Enemy) && _hasPowerup)
        {
            Rigidbody enemyRigidbody = collision.gameObject.GetComponent<Rigidbody>();
            Vector3 awayFromPlayer = collision.gameObject.transform.position - transform.position;
            Debug.Log($"Collided with {collision.gameObject.name} with powerup set to {_hasPowerup}");
            enemyRigidbody.AddForce(awayFromPlayer * PowerupStrength, ForceMode.Impulse);
        }
    }
}