using UnityEngine;
using UnityEngine.Serialization;

public class BallController : MonoBehaviour
{
    [FormerlySerializedAs("_rb")] [SerializeField] public Rigidbody Rb;
    [SerializeField] private float _speed;

    private void Start()
    {
        Rb.AddForce(10,0,10 * 3, ForceMode.Impulse);
    }

    private void FixedUpdate()
    {
        Vector3 currentVelocity = Rb.velocity.normalized;
        Rb.velocity = currentVelocity * _speed;
    }
}
