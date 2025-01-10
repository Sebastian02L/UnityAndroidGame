using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    //Elementos de la interfaz
    [SerializeField] TextMeshProUGUI countDownText;
    [SerializeField] TextMeshProUGUI distanceText;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

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
}
