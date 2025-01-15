using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    //Elementos de la interfaz
    [SerializeField] TextMeshProUGUI countDownText;
    [SerializeField] TextMeshProUGUI distanceText;
    [SerializeField] GameObject resultsScreen;
    [SerializeField] TextMeshProUGUI endGameText;

    //Metodo para actualizar el texto de la cuenta atras del inicio de la partida
    public void UpdateCountdown(string value)
    {
        countDownText.text = value;
    }

    //Metodo que oculta el texto de la cuenta atras
    public void HideCountdown()
    {
        countDownText.gameObject.SetActive(false);
    }

    //Metodo que actualiza la distancia recorrida por el jugador
    public void UpdateDistance(string distance) 
    {
        distanceText.text = "Distancia: " + distance + "m";
    }

    public void EnableEndGameText(string distance)
    {
        endGameText.text = "Has recorrido " + distance + "m";
        distanceText.gameObject.SetActive(false);
    }

    //Metodo para cargar la escena especificada por su id
    public void LoadNextScene(int id)
    {
        Time.timeScale = Time.timeScale == 0 ? 1 : 1; //Si el tiempo esta parado, lo reanudamos (puede estar parado por el fin de la partida o por juego pausado)
        SceneManager.LoadScene(id);
    }

    public void ShowResultsScreen(string totalDistance)
    {
        resultsScreen.SetActive(true);
        endGameText.text = totalDistance + "m";
    }
}
