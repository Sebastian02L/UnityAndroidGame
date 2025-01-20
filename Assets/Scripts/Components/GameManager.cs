using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //Referencia a GameObjects de la escena, concretamente al UIManager, al grupo de objetos y el punto de salida del jugador
    [SerializeField] UIManager UI;
    [SerializeField] GameObject groupGO; //Area de spawn y jugador
    [SerializeField] GameObject startingPosition;

    //Distancia de la zona de aparicion respecto al jugador y variables necesarias
    [Header("Configuración de la Zona de Aparicion")]
    [SerializeField] private float spawnAreaDistance; 
    [SerializeField] GameObject spawnArea;
    [SerializeField] List<GameObject> obstaclePrefabs;
    [SerializeField] GameObject coinPrefab;
    float baseSpawnAreaDistance; 
    private float previousDistance; 

    //Puntos de aparicion de los objetos y el intervalo de aparicion
    [SerializeField] List<GameObject> spawnPoints; 
    [SerializeField] float spawnInterval; 
    float spawnTimer = 0f; 

    //Numero minimo y maximo de puntos de aparicion libres, cambiando cada vez que aumente la dificultad
    [SerializeField] int minFreeSpawnPoints;
    [SerializeField] int maxFreeSpawnPoints;

    //Listas para almacenar los puntos libres de aparicion y recordar los puntos libres de la ronda anterior para darle prioridad a los puntos que no estaban libres
    List<int> actualFreePoints = new List<int>();
    List<int> latestFreePoints = new List<int>();

    [Header("Configuración de la dificultad")]
    [SerializeField] float distanceGoalInterval; //Cada x metros aumenta la dificultad
    [SerializeField] float nextGoal;
    [SerializeField] float speedPorcentageIncrement; //Porcentaje de incremento de la velocidad del jugador
    float baseSpeed; //Velocidad base del jugador, se utiliza para aumentar siempre la misma cantidad en la velocidad 

    //Datos del jugador
    [SerializeField] float playerSpeed;
    float totalPlayerDistance;

    //Variable para indicar cuando la partida ha comenzado
    bool gameStarted = false;

    private void Awake()
    {
        //Moveremos a una distancia x la zona de aparicion de los objetos respecto al jugador
        //Usando el eje z como direccion del movimiento, al ya estar normalizado, solo debemos multiplicarlo por la distancia y sumar esto a la posicion de la zona
        spawnArea.transform.position += Vector3.forward * spawnAreaDistance;
        previousDistance = spawnAreaDistance;

        //Comprobamos que haya desplazado correctamente la zona
        Vector3 distanceBetween = GameObject.FindWithTag("Player").transform.position - spawnArea.transform.position;
        //Debug.Log("Distance: " + distanceBetween.magnitude);

        //Inicializamos la primera meta y la velocidad base
        nextGoal = distanceGoalInterval;
        baseSpeed = playerSpeed;
        baseSpawnAreaDistance = spawnAreaDistance;
    }

    void Start()
    {
        //Cuando comience la partida, activamos una cuenta atras con una corutina
        StartCoroutine(StartGame());
    }

    //Metodo que realiza la cuanta atras al inicio de la partida
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

    public void EndGame()
    {
        UI.ShowResultsScreen();
        PlayerDataManager.Instance.SetPoints((int)totalPlayerDistance);
    }

    void Update()
    {
        if (!gameStarted) return;

        UpdateSpawnAreaDistance();
        UpdateDifficulty();

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

    //Metodo que actualiza la distancia de la zona de aparicion de los objetos
    void UpdateSpawnAreaDistance()
    {
        if (previousDistance == spawnAreaDistance)
        {
            return;
        }
        //Si se cambia la distancia
        float distanceVariation = spawnAreaDistance - previousDistance;
        spawnArea.transform.position += Vector3.forward * distanceVariation;
        previousDistance = spawnAreaDistance;

        //Comprobamos que haya desplazado correctamente la zona
        Vector3 distanceBetween = GameObject.FindWithTag("Player").transform.position - spawnArea.transform.position;
        //Debug.Log("Distance: " + distanceBetween.magnitude);
    }

    //Metodo que mueve al grupo de objetos en conjunto (jugador y area de spawn)
    void MoveGroup() 
    { 
        groupGO.transform.position += Vector3.forward * playerSpeed * Time.fixedDeltaTime;

        //Calculamos el vector entre el jugador y el punto de aparicion, su magnitud sera la distancia recorrida por el jugador
        Vector3 vector = groupGO.transform.position - startingPosition.transform.position;
        totalPlayerDistance = vector.magnitude;
        UI.UpdateDistance((int)totalPlayerDistance);
    }

    //Metodo que determina los puntos libres del area de spawn
    void SetFreeSpawnPoints()
    {
        //Determinamos de manera aleatoria el numero de puntos libres
        int numberOfFreePoints = Random.Range(minFreeSpawnPoints, maxFreeSpawnPoints + 1);
        int count = 0;

        //Si es la primera vez que se ejecuta
        if (latestFreePoints.Count == 0)
        {
            while (count != numberOfFreePoints)
            {
                int index = Random.Range(0, spawnPoints.Count);
                if (!actualFreePoints.Contains(index))
                {
                    actualFreePoints.Add(index);
                    count++;
                }
            }

            latestFreePoints = new List<int>(actualFreePoints);
        }
        else //Se evita que puedan repetirse los puntos libres entre "rondas"
        {
            while (count != numberOfFreePoints)
            {
                int index = Random.Range(0, spawnPoints.Count);

                if (!latestFreePoints.Contains(index))
                {
                    actualFreePoints.Add(index);
                    count++;
                }
                else
                {
                    int decision = Random.Range(0, 2);
                    if (decision == 0)
                    {
                        actualFreePoints.Add(index);
                        count++;
                    }
                }
            latestFreePoints = new List<int>(actualFreePoints);
        }
    }

    //Metodo que instancia los obstaculos una vez determinados los puntos libres
    void SpawnObjects() 
    {
        for (int i = 0; i < spawnPoints.Count; i++)
        {
            if(!actualFreePoints.Contains(i))
            {
                //GameObject GObject = Instantiate(GameObject.CreatePrimitive(PrimitiveType.Cube), spawnPoints[i].transform.position, Quaternion.identity);
                //GObject.AddComponent<BoxCollider>();
                //GObject.transform.SetParent(null);
                //Destroy(GObject.gameObject, 10f);

                //Escoge de forma aleatoria un obstaculo de la lista y lo instancia en el punto de spawn
                int index = Random.Range(0, obstaclePrefabs.Count);
                GameObject obstacle = Instantiate(obstaclePrefabs[index], spawnPoints[i].transform.position, spawnPoints[i].transform.rotation);
            }
            else
            {
                if (Random.Range(0, 10) == 0) Instantiate(coinPrefab, spawnPoints[i].transform.position, spawnPoints[i].transform.rotation);
            }
        }
    }

    //Metodo que limpia la lista de puntos libres 
    void SetUpNextRound() 
    {
        actualFreePoints.Clear();
    }

    void UpdateDifficulty()
    {
        //Si alcanza la meta de distancia, aumentamos la dificultad
        if (totalPlayerDistance >= nextGoal)
        {
            nextGoal += distanceGoalInterval;
            playerSpeed += baseSpeed * speedPorcentageIncrement;
            spawnAreaDistance += baseSpawnAreaDistance * speedPorcentageIncrement;

            if (minFreeSpawnPoints > 1) minFreeSpawnPoints--;
            if (maxFreeSpawnPoints > 1) maxFreeSpawnPoints--;
            if (spawnInterval > 0.5f) spawnInterval -= 0.05f;
        }
    }

    //Metodo para pausar el juego
    public void PauseGame(bool value)
    {
        Time.timeScale = value ? 0f : 1f;
    }

    public void SetGameState(bool state)
    {
        gameStarted = state;
    }
}
