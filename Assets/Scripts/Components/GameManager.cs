using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //Referencia a GameObjects de la escena, concretamente al UIManager, al grupo de objetos y el punto de salida del jugador
    [SerializeField] UIManager UI;
    [SerializeField] GameObject groupGO; 
    [SerializeField] GameObject startingPosition;

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
}
