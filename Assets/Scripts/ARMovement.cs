using TMPro;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ARMovement : MonoBehaviour
{
    [Header("XR CONFIGURATION: ")]
    [SerializeField] private ARFaceManager _faceTracker; //Tracker de la cara
    [SerializeField] private TextMeshProUGUI _alertText; //Alerta de no detecci�n facial

    [Header("Speeds: ")]
    [SerializeField] private float _baseMovementSpeed = 10; //Velocidad base de movimiento en X
    [SerializeField] private float _speedRotationInfluence = 1.5f; //Multiplicador que hace afecta a la diferencia de velocidad dependiendo de la inclinaci�n del jugador

    [Header("Limits: ")]
    [SerializeField] private float _maxRotation = 45; //Rotaci�n m�xima que puede efectuar el personaje
    [SerializeField] private float _boundsDistance = 3; //Distancia a los extremos del mapa
    [SerializeField] private float _deadZoneValue = 5; //Angulaci�n m�xima que no provoca un movimiento lateral

    private Rigidbody _rb;
    private float _rotationZ = 0; //Rotaci�n de la cabeza en el eje Z actualmente

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (_faceTracker.trackables.count == 0)
        {
            _alertText.gameObject.SetActive(true);
            return;
        }

        _alertText.gameObject.SetActive(false);
        foreach(ARFace face in _faceTracker.trackables) //El m�ximo n�mero de caras est� configurado a 1, pero iterar es la �nica forma de obtener las caras
        {
            _rotationZ = -face.transform.rotation.eulerAngles.z; //Hay que negarlo porque si no gira al rev�s desde el punto de vista del jugador
            if (_rotationZ < -180) _rotationZ += 360; //Se hace debido a la forma que tiene ARFace de devolver la rotaci�n
            _rotationZ = Mathf.Clamp(_rotationZ, -_maxRotation, _maxRotation);
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, _rotationZ);
        }
    }

    private void FixedUpdate() //Aqu� se calcula el movimiento
    {
        if (Mathf.Abs(_rotationZ) > _deadZoneValue) //Tiene que superar el umbral de _deadZoneValue para efectuar movimiento
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
