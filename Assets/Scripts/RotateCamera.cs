using UnityEngine;

public class RotateCamera : MonoBehaviour
{
    private const float RotationSpeed = 50;

    private void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        transform.Rotate(Vector3.up, horizontalInput * RotationSpeed * Time.deltaTime);
    }
}