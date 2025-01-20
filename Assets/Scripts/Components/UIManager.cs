using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    //Elementos de la interfaz
    [SerializeField] TextMeshProUGUI countDownText;
    [SerializeField] TextMeshProUGUI distanceText;
    [SerializeField] GameObject resultsScreen;
    [SerializeField] TextMeshProUGUI resultsDistanceText;
    [SerializeField] TextMeshProUGUI resultsCoinsText;
    [SerializeField] AudioManager audioManager;
    int distance = 0;
    int coins = 0;

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
    public void UpdateDistance(int distance) 
    {
        this.distance = distance;
        distanceText.text = "DISTANCE: " + distance.ToString() + "m";
    }

    public void UpdateCoins(int coins)
    {
        this.coins += coins;
    }

    public void EnableEndGameText(string distance)
    {
        resultsDistanceText.text = distance + "m";
        distanceText.gameObject.SetActive(false);
    }

    //Metodo para cargar la escena especificada por su id
    public void LoadNextScene(int id)
    {
        Time.timeScale = Time.timeScale == 0 ? 1 : 1; //Si el tiempo esta parado, lo reanudamos (puede estar parado por el fin de la partida o por juego pausado)
        SceneManager.LoadScene(id);
    }

    public void ReloadScene()
    {
        Time.timeScale = Time.timeScale == 0 ? 1 : 1; //Si el tiempo esta parado, lo reanudamos (puede estar parado por el fin de la partida o por juego pausado)
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ShowResultsScreen()
    {
        resultsScreen.SetActive(true);
        resultsDistanceText.text = distance + "m";
        resultsCoinsText.text = coins + "coins";
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
