using UnityEditor.SceneManagement;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject[] prefabs; // Array de prefabs (los tres escenarios)
    public float spawnDistance = 20f; // Distancia a la que se generarán los prefabs
    public Transform player; // Referencia al jugador
    public float Ydistance = 10;//altura del prefab
    public float Xdistance = -7;//posición horizontal del prefab
    private float lastSpawnZ = 0f; // Última posición Z donde se generó un prefab

    private void Start()
    {
        float planeSize;
        // Obtén el hijo Plane del primer objeto para poder coger el size en z
        Transform planeTransform = prefabs[0].transform.Find("Plane"); 

        // Obtener el tamaño del Plane
        // Suponiendo que Plane tiene un componente Renderer
        Renderer planeRenderer = planeTransform.GetComponent<Renderer>();
        if (planeRenderer != null)
        {
            //obtener size.z y asignarlo a la distancia de spawn
            planeSize = planeRenderer.bounds.size.z;
            Debug.Log("Tamaño del Plane: " + planeSize);
            spawnDistance = planeSize /2;
        }
        else
        {
            Debug.LogError("El objeto Plane no tiene un componente Renderer.");
        }

        
    }
    void Update()
    {
        // Verificar si el jugador ha avanzado lo suficiente
        if (player.position.z >= lastSpawnZ + spawnDistance)
        {
            SpawnObstacle();
        }
    }

    void SpawnObstacle()
    {
        // Seleccionar un prefab aleatorio
        GameObject prefabToSpawn = prefabs[Random.Range(0, prefabs.Length)];

        // Calcular la posición Z para el nuevo prefab
        float spawnZ = lastSpawnZ + spawnDistance;

        // Verificar si hay algún objeto en la distancia de 20 unidades en la dirección Z
        if (!IsObstacleInRange(spawnZ))
        {
            // Instanciar el prefab en la posición (Xdistance, Ydistance, spawnZ)
            Instantiate(prefabToSpawn, new Vector3(Xdistance, Ydistance, spawnZ), Quaternion.identity);
            lastSpawnZ = spawnZ; // Actualizar la última posición Z
        }
    }

    bool IsObstacleInRange(float spawnZ)
    {
        // Comprobar si hay obstáculos en la distancia de 20 unidades
        Collider[] hitColliders = Physics.OverlapSphere(new Vector3(Xdistance, Ydistance, spawnZ), 10f);
        foreach (var hitCollider in hitColliders)
        {
            // Verificar si el objeto colisionado es un prefab de la lista
            foreach (var prefab in prefabs)
            {
                if (hitCollider.gameObject.CompareTag(prefab.tag))
                {
                    return true; // Hay un obstáculo en el rango
                }
            }
        }
        return false; // No hay obstáculos en el rango
    }
}