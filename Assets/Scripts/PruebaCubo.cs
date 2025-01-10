using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class PruebaCubo : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 10;
    [SerializeField] private ARHumanBodyManager arHumanBodyManager;

    void Update()
    {
        transform.Rotate(Vector3.one * rotationSpeed * Time.deltaTime);

        if (arHumanBodyManager != null && arHumanBodyManager.subsystem != null && arHumanBodyManager.subsystem.running)
        {
            GetComponent<Renderer>().material.color = Color.green;
        }
        else
        {
            GetComponent<Renderer>().material.color = Color.red;
        }
    }

    public void ToggleVisibility()
    {
        GetComponent<MeshRenderer>().enabled = false;
    }


}
