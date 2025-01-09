using UnityEngine;

//Esta clase es para poder mover desde el inspector la zona de aparicion de los objetos, poniendo una distancia especifica para probar y encontrar la distancia final
public class SpawnAreaDistanceComponent : MonoBehaviour
{
    [SerializeField] private float distance;
    private float previousDistance;

    void Awake()
    {
        //Moveremos a una distancia x la zona de aparicion de los objetos respecto al jugador
        //Usando el eje z como direccion del movimiento, al ya estar normalizado, solo debemos multiplicarlo por la distancia y sumar esto a la posicion de la zona
        transform.position += Vector3.forward * distance;
        previousDistance = distance;

        //Comprobamos que haya desplazado correctamente la zona
        Vector3 distanceBetween = GameObject.FindWithTag("Player").transform.position - transform.position;
        Debug.Log("Distance: " + distanceBetween.magnitude);

    }

    void Update()
    {
        if(previousDistance == distance)
        {
            return;
        }
        //Si se cambia en tiempo de ejecucion desde el inspector
        float distanceVariation = distance - previousDistance;
        transform.position += Vector3.forward * distanceVariation;
        previousDistance = distance;

        //Comprobamos que haya desplazado correctamente la zona
        Vector3 distanceBetween = GameObject.FindWithTag("Player").transform.position - transform.position;
        Debug.Log("Distance: " + distanceBetween.magnitude);
    }
}
