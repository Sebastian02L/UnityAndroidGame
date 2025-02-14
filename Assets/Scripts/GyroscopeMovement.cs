using UnityEngine;

public class GyroscopeMovement : MonoBehaviour
{
    [Header("Speeds: ")]
    [SerializeField] private float _baseMovementSpeed = 10; //Velocidad base de movimiento en X
    [SerializeField] private float _rotationSpeed = 50; //Velocidad de rotaci�n
    [SerializeField] private float _speedRotationInfluence = 1.5f; //Multiplicador que hace afecta a la diferencia de velocidad dependiendo de la inclinaci�n del jugador
    
    [Header("Limits: ")]
    [SerializeField] private float _maxRotation = 45; //Rotaci�n m�xima que puede efectuar el personaje
    [SerializeField] private float _boundsDistance = 3; //Distancia a los extremos del mapa
    [SerializeField] private float _deadZoneValue = 5; //Angulaci�n m�xima que no provoca un movimiento lateral

    private Rigidbody _rb;
    private float _rotationZ = 0; //Rotaci�n almacenada en Z del movimiento del dispositivo
    private float _zClamped = 0; 

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        //Los dispositivos Android de base tienen el giroscopio desactivado
        Input.gyro.enabled = true;
    }

    void Update() //Aqu� se calcula la rotaci�n
    {
        if (!Input.gyro.enabled) return;
        //_rotationZ suma a s� misma la rotaci�n del dispositivo en el eje Z y controla que el personaje no gire demasiado
        _rotationZ += Input.gyro.rotationRateUnbiased.z * _rotationSpeed * Time.deltaTime;
        _zClamped = Mathf.Clamp(_rotationZ, -_maxRotation, _maxRotation);
        //Se aplica la rotaci�n
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, _zClamped);
    }

    private void FixedUpdate() //Aqu� se calcula el movimiento
    {
        if(Mathf.Abs(_zClamped) > _deadZoneValue) //Tiene que superar el umbral de _deadZoneValue para efectuar movimiento
        {
            float normalizedRotation = Mathf.InverseLerp(0, 45, Mathf.Abs(_zClamped)); //La rotaci�n se remapea a un valor entre 0 y 1
            float speedMultiplier = Mathf.Pow(normalizedRotation, 2) * _speedRotationInfluence; //el multiplicador de velocidad es exponencial, afectado por _speedRotationInfluence
            _rb.linearVelocity = new Vector3(-Mathf.Sign(_zClamped) * _baseMovementSpeed * speedMultiplier, 0, 0); //Se aplica la velocidad linear
        }
        else _rb.linearVelocity = Vector3.zero; //Si est� en la dead zone no se mueve
        //Se controla que el jugador no se salga de los l�mites del mapa
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -_boundsDistance, _boundsDistance), transform.position.y, transform.position.z);
    }
}
