using UnityEngine;

public class BallController : MonoBehaviour
{
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private float _speed;

    private void Start()
    {
        _rb.AddForce(10,0,10 * 3, ForceMode.Impulse);
    }

    private void FixedUpdate()
    {
        Vector3 currentVelocity = _rb.velocity.normalized;
        _rb.velocity = currentVelocity * _speed;
    }
}
