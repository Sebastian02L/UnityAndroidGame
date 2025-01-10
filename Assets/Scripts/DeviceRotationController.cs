using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class DeviceRotationController : MonoBehaviour
{
    [SerializeField] private float _baseMovementSpeed = 10;
    [SerializeField] private float _speedRotationInfluence = 2;
    [SerializeField] private float _rotationSpeed = 100;
    [SerializeField] private float _boundsDistance = 4;
    [SerializeField] private float _deadZoneValue = 5;
    private Rigidbody _rb;
    private float _rotationZ = 0;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (!Input.gyro.enabled) Input.gyro.enabled = true;
        _rotationZ += Input.gyro.rotationRateUnbiased.z * _rotationSpeed * Time.deltaTime;
        _rotationZ = Mathf.Clamp(_rotationZ, -45, 45);
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, _rotationZ);
    }

    private void FixedUpdate()
    {
        if(Mathf.Abs(_rotationZ) > _deadZoneValue)
        {
            float normalizedRotation = Mathf.InverseLerp(0, 45, Mathf.Abs(_rotationZ));
            float speedMultiplier = Mathf.Pow(normalizedRotation, 2) * _speedRotationInfluence;
            _rb.linearVelocity = new Vector3(-Mathf.Sign(_rotationZ) * _baseMovementSpeed * speedMultiplier, 0, 0);
        }
        else _rb.linearVelocity = Vector3.zero;
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -_boundsDistance, _boundsDistance), transform.position.y, transform.position.z);
    }
}
