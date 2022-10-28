using UnityEngine;

public class RocketBehaviour : MonoBehaviour
{
    private const float Speed = 15.0f;
    private const float RocketStrength = 15.0f;
    private const float AliveTimer = 5.0f;

    private Transform _target;
    private bool _homing;

    public void Fire(Transform newTarget)
    {
        _target = newTarget;
        _homing = true;
        Destroy(gameObject, AliveTimer);
    }

    private void Update()
    {
        if (_homing)
        {
            if (_target == null)
            {
                Destroy(gameObject);
            }
            else
            {
                Vector3 moveDirection = (_target.transform.position - transform.position).normalized;
                transform.position += Speed * Time.deltaTime * moveDirection;
                transform.LookAt(_target);
            }
        }
    }

    private void OnCollisionEnter(Collision col)
    {
        if (_target == null) return;
        if (col.gameObject.CompareTag(_target.tag))
        {
            Rigidbody targetRigidbody = col.gameObject.GetComponent<Rigidbody>();
            Vector3 away = -col.contacts[0].normal;
            targetRigidbody.AddForce(away * RocketStrength, ForceMode.Impulse);
            Destroy(gameObject);
        }
    }
}