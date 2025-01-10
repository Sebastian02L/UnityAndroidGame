using System.Collections;
using System.Collections.Generic;
using System.Threading;
using NUnit.Framework;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //Referencia a GameObjects de la escena, concretamente al UIManager, al grupo de objetos y el punto de salida del jugador
    [SerializeField] UIManager UI;
    [SerializeField] GameObject groupGO; 
    [SerializeField] GameObject startingPosition;

    [Header("Objetos del Sistema de Aparición")]
    [SerializeField] List<GameObject> spawnPoints;
    [SerializeField] float spawnInterval;
    float spawnTimer = 0f;

    [SerializeField] int minFreeSpawnPoints;
    [SerializeField] int maxFreeSpawnPoints;

    List<int> freePoints = new List<int>();


    //Velocidad del jugador
    [SerializeField] float playerSpeed;

    //Variable para indicar cuando la partida ha comenzado
    bool gameStarted = false;


    void Start()
    {
        //Cuando comience la partida, activamos una cuenta atras con una corutina
        StartCoroutine(StartGame());
    }

    IEnumerator StartGame()
    {
        UI.UpdateCountdown("3");
        yield return new WaitForSeconds(1f);
        UI.UpdateCountdown("2");
        yield return new WaitForSeconds(1f);
        UI.UpdateCountdown("1");
        yield return new WaitForSeconds(1f);
        UI.HideCountdown();

        gameStarted = true;
    }

    void Update()
    {
        if (!gameStarted) return;

        //Gestion de la partida

        spawnTimer += Time.deltaTime;
        if (spawnTimer >= spawnInterval)
        {
            //Determinar los puntos que no van a spawnear objetos
            SetFreeSpawnPoints();

            //Spawneo de objetos
            SpawnObjects();

            //Limpiamos la lista de puntos libres y comprobamos si hay que aumentar la dificultad
            SetUpNextRound();

            spawnTimer = 0f;
        }
    }

    private void FixedUpdate()
    {
        if (!gameStarted) return;

        //Movemos al grupo de objetos
        MoveGroup();
    }

    void MoveGroup() 
    { 
        groupGO.transform.position += Vector3.forward * playerSpeed * Time.fixedDeltaTime;

        //Calculamos el vector entre el jugador y el punto de aparicion, su magnitud sera la distancia recorrida por el jugador
        Vector3 vector = groupGO.transform.position - startingPosition.transform.position;
        UI.UpdateDistance(vector.magnitude.ToString("F0"));
    }

    void SetFreeSpawnPoints()
    {
        //Determinamos de manera aleatoria el numero de puntos libres
        int numberOfFreePoints = Random.Range(minFreeSpawnPoints, maxFreeSpawnPoints + 1);
        int count = 0;

        while(count != numberOfFreePoints) 
        {
            int index = Random.Range(0, spawnPoints.Count);
            if(!freePoints.Contains(index))
            {
                freePoints.Add(index);
                count++;
            }
        }
    }

    void SpawnObjects() 
    {
        for (int i = 0; i < spawnPoints.Count; i++)
        {
            if(!freePoints.Contains(i))
            {
                GameObject GObject = Instantiate(GameObject.CreatePrimitive(PrimitiveType.Cube), spawnPoints[i].transform.position, Quaternion.identity);
                GObject.transform.SetParent(null);
                Destroy(GObject.gameObject, 5f);
            }
        }
    }

    void SetUpNextRound() 
    {
        freePoints.Clear();
    }
}
