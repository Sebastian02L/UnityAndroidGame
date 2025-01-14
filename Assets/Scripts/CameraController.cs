using UnityEngine;

public class CameraController : MonoBehaviour
{
    private WebCamTexture webCamTexture;

    void Start()
    {
        foreach (WebCamDevice device in WebCamTexture.devices) //Se obtiene la cámara frontal del móvil
        {
            if (device.isFrontFacing)
            {
                webCamTexture = new WebCamTexture(device.name);
                break;
            }
        }
        if (webCamTexture != null)
        {
            webCamTexture.Play(); //Inicializa la cámara
            //GetComponent<Renderer>().material.mainTexture = webCamTexture; -> ESTO ERA PARA HACER LA PRUEBA
            Debug.Log("Cámara encontrada correctamente.");
        }
        else
        {
            Debug.LogError("No se encontró una cámara frontal en el dispositivo. Número de cámaras: " + WebCamTexture.devices.Length);
        }
    }

    public Texture2D GetCameraTexture() //Será usada más adelante por el modelo de reconocimiento para calcular la posición de las manos
    {
        Texture2D texture = new Texture2D(webCamTexture.width, webCamTexture.height);
        texture.SetPixels(webCamTexture.GetPixels());
        texture.Apply();
        return texture;
    }
}