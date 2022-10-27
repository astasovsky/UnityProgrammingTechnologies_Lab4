using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform focalPoint;

    private const float Speed = 500;
    private Rigidbody _playerRigidbody;

    private void Start()
    {
        _playerRigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        float forwardInput = Input.GetAxis("Vertical");
        _playerRigidbody.AddForce(Speed * forwardInput * Time.deltaTime * focalPoint.transform.forward);
    }
}