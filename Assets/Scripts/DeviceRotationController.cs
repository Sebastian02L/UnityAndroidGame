using UnityEngine;

public class DeviceRotationController : MonoBehaviour
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

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    void Update() //Aqu� se calcula la rotaci�n
    {
        //Algunos dispositivos por defecto no tienen el giroscopio habilitado de base
        if (!Input.gyro.enabled) Input.gyro.enabled = true; //Si lo ponemos en el Start a veces da error y no lo activa, as� que de momento se queda aqu�
        //_rotationZ suma a s� misma la rotaci�n del dispositivo en el eje Z y controla que el personaje no gire demasiado
        _rotationZ += Input.gyro.rotationRateUnbiased.z * _rotationSpeed * Time.deltaTime;
        _rotationZ = Mathf.Clamp(_rotationZ, -_maxRotation, _maxRotation);
        //Se aplica la rotaci�n
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, _rotationZ);
    }

    private void FixedUpdate() //Aqu� se calcula el movimiento
    {
        if(Mathf.Abs(_rotationZ) > _deadZoneValue) //Tiene que superar el umbral de _deadZoneValue para efectuar movimiento
        {
            float normalizedRotation = Mathf.InverseLerp(0, 45, Mathf.Abs(_rotationZ)); //La rotaci�n se remapea a un valor entre 0 y 1
            float speedMultiplier = Mathf.Pow(normalizedRotation, 2) * _speedRotationInfluence; //el multiplicador de velocidad es exponencial, afectado por _speedRotationInfluence
            _rb.linearVelocity = new Vector3(-Mathf.Sign(_rotationZ) * _baseMovementSpeed * speedMultiplier, 0, 0); //Se aplica la velocidad linear
        }
        else _rb.linearVelocity = Vector3.zero; //Si est� en la dead zone no se mueve
        //Se controla que el jugador no se salga de los l�mites del mapa
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -_boundsDistance, _boundsDistance), transform.position.y, transform.position.z);
    }
}
