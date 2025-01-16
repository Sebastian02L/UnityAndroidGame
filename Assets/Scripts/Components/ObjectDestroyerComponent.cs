using UnityEngine;

public class ObjectDestroyerComponent : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Destroying object: " + other.gameObject.name);
        Destroy(other.gameObject);
    }
}
