using System.Collections;
using UnityEngine;

public class MachinistMovement : MonoBehaviour
{
    [Header("Speeds: ")]
    [SerializeField] private float _baseMovementSpeed = 10; //Velocidad base de movimiento en X
    [SerializeField] private float _rotationSpeed = 50; //Velocidad de rotación
    [SerializeField] private float _speedRotationInfluence = 1.5f; //Multiplicador que hace afecta a la diferencia de velocidad dependiendo de la inclinación del jugador

    [Header("Limits: ")]
    [SerializeField] private float _maxRotation = 45; //Rotación máxima que puede efectuar el personaje
    [SerializeField] private float _boundsDistanceX = 6.25f; //Distancia a los extremos del mapa
    [SerializeField] private float _boundsDistanceY = 2f; 
    [SerializeField] private float _deadZoneValue = 5; //Angulación máxima que no provoca un movimiento lateral

    [Header("Camera: ")]
    [SerializeField] private Transform _camera;
    [SerializeField] private Transform[] _camTransforms;
    [SerializeField] private float _slerpSpeed = 10;
    [SerializeField] private float _lerpSpeed = 10;
    [SerializeField] private GameObject _alertCanvas;
    private int _currentTransform = 0;

    private Rigidbody _rb;
    private float _rotationZ = 0; //Rotación almacenada en Z del movimiento del dispositivo
    private float _zClamped = 0;
    private float _rotationX = 0; //Rotación almacenada en X del movimiento del dispositivo
    private float _xClamped = 0;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        //Los dispositivos Android de base tienen el giroscopio desactivado
        StartCoroutine(EnableGyro());
    }

    IEnumerator EnableGyro()
    {
        Input.gyro.enabled = false;
        yield return new WaitForSeconds(3f);
        Input.gyro.enabled = true;
        _alertCanvas.SetActive(false);
    }

    void Update() //Aquí se calcula la rotación
    {
        CheckCameraChange();
        if (!Input.gyro.enabled) return;
        _rotationZ += Input.gyro.rotationRateUnbiased.z * _rotationSpeed * Time.deltaTime;
        _zClamped = Mathf.Clamp(_rotationZ, -_maxRotation, _maxRotation);
        _rotationX += -Input.gyro.rotationRateUnbiased.x * _rotationSpeed * Time.deltaTime;
        _xClamped = Mathf.Clamp(_rotationX, -_maxRotation, _maxRotation);
        //Se aplica la rotación
        transform.rotation = Quaternion.Euler(_xClamped, transform.rotation.eulerAngles.y, _zClamped);
    }

    private void FixedUpdate() //Aquí se calcula el movimiento
    {
        float ySpeed = 0;
        float xSpeed = 0;
        float normalizedRotation;
        float speedMultiplier;
        if (Mathf.Abs(_zClamped) > _deadZoneValue) //Tiene que superar el umbral de _deadZoneValue para efectuar movimiento
        {
            normalizedRotation = Mathf.InverseLerp(0, 45, Mathf.Abs(_zClamped)); //La rotación se remapea a un valor entre 0 y 1
            speedMultiplier = Mathf.Pow(normalizedRotation, 2) * _speedRotationInfluence; //el multiplicador de velocidad es exponencial, afectado por _speedRotationInfluence
            xSpeed = -Mathf.Sign(_zClamped) * _baseMovementSpeed * speedMultiplier;
        }
        if (Mathf.Abs(_xClamped) > _deadZoneValue) //Tiene que superar el umbral de _deadZoneValue para efectuar movimiento
        {
            normalizedRotation = Mathf.InverseLerp(0, 45, Mathf.Abs(_xClamped)); //La rotación se remapea a un valor entre 0 y 1
            speedMultiplier = Mathf.Pow(normalizedRotation, 2) * _speedRotationInfluence; //el multiplicador de velocidad es exponencial, afectado por _speedRotationInfluence
            ySpeed = -Mathf.Sign(_xClamped) * _baseMovementSpeed * speedMultiplier;
        }
        _rb.linearVelocity = new Vector3(xSpeed, ySpeed, 0);
        //Se controla que el jugador no se salga de los límites del mapa
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -_boundsDistanceX, _boundsDistanceX), Mathf.Clamp(transform.position.y, -_boundsDistanceY, _boundsDistanceY), transform.position.z);
    }

    private void CheckCameraChange()
    {
        _camera.position = Vector3.Lerp(_camera.position, _camTransforms[_currentTransform].position, _lerpSpeed * Time.deltaTime);
        _camera.rotation = Quaternion.Slerp(_camera.rotation, _camTransforms[_currentTransform].rotation, _slerpSpeed * Time.deltaTime);

        if (Input.touchCount > 0)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                _currentTransform = (_currentTransform == 0) ? 1 : 0;
            }
        }
    }
}
