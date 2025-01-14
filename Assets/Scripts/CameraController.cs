using UnityEngine;

public class CameraController : MonoBehaviour
{
    private WebCamTexture webCamTexture;

    void Start()
    {
        foreach (WebCamDevice device in WebCamTexture.devices) //Se obtiene la c�mara frontal del m�vil
        {
            if (device.isFrontFacing)
            {
                webCamTexture = new WebCamTexture(device.name);
                break;
            }
        }
        if (webCamTexture != null)
        {
            webCamTexture.Play(); //Inicializa la c�mara
            //GetComponent<Renderer>().material.mainTexture = webCamTexture; -> ESTO ERA PARA HACER LA PRUEBA
            Debug.Log("C�mara encontrada correctamente.");
        }
        else
        {
            Debug.LogError("No se encontr� una c�mara frontal en el dispositivo. N�mero de c�maras: " + WebCamTexture.devices.Length);
        }
    }

    public Texture2D GetCameraTexture() //Ser� usada m�s adelante por el modelo de reconocimiento para calcular la posici�n de las manos
    {
        Texture2D texture = new Texture2D(webCamTexture.width, webCamTexture.height);
        texture.SetPixels(webCamTexture.GetPixels());
        texture.Apply();
        return texture;
    }
}