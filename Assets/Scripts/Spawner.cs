using UnityEditor.SceneManagement;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject[] prefabs; // Array de prefabs (los tres escenarios)
    public float spawnDistance = 20f; // Distancia a la que se generar�n los prefabs
    public Transform player; // Referencia al jugador
    public float Ydistance = 10;//altura del prefab
    public float Xdistance = -7;//posici�n horizontal del prefab
    private float lastSpawnZ = 0f; // �ltima posici�n Z donde se gener� un prefab

    private void Start()
    {
        float planeSize;
        // Obt�n el hijo Plane del primer objeto para poder coger el size en z
        Transform planeTransform = prefabs[0].transform.Find("Plane"); 

        // Obtener el tama�o del Plane
        // Suponiendo que Plane tiene un componente Renderer
        Renderer planeRenderer = planeTransform.GetComponent<Renderer>();
        if (planeRenderer != null)
        {
            //obtener size.z y asignarlo a la distancia de spawn
            planeSize = planeRenderer.bounds.size.z;
            Debug.Log("Tama�o del Plane: " + planeSize);
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

        // Calcular la posici�n Z para el nuevo prefab
        float spawnZ = lastSpawnZ + spawnDistance;

        // Verificar si hay alg�n objeto en la distancia de 20 unidades en la direcci�n Z
        if (!IsObstacleInRange(spawnZ))
        {
            // Instanciar el prefab en la posici�n (Xdistance, Ydistance, spawnZ)
            Instantiate(prefabToSpawn, new Vector3(Xdistance, Ydistance, spawnZ), Quaternion.identity);
            lastSpawnZ = spawnZ; // Actualizar la �ltima posici�n Z
        }
    }

    bool IsObstacleInRange(float spawnZ)
    {
        // Comprobar si hay obst�culos en la distancia de 20 unidades
        Collider[] hitColliders = Physics.OverlapSphere(new Vector3(Xdistance, Ydistance, spawnZ), 10f);
        foreach (var hitCollider in hitColliders)
        {
            // Verificar si el objeto colisionado es un prefab de la lista
            foreach (var prefab in prefabs)
            {
                if (hitCollider.gameObject.CompareTag(prefab.tag))
                {
                    return true; // Hay un obst�culo en el rango
                }
            }
        }
        return false; // No hay obst�culos en el rango
    }
}