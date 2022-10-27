using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform focalPoint;

    private const float Speed = 5;
    private Rigidbody _playerRigidbody;

    private void Start()
    {
        _playerRigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        float forwardInput = Input.GetAxis("Vertical");
        _playerRigidbody.AddForce(Speed * forwardInput * focalPoint.transform.forward);
    }
}