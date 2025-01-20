using UnityEngine;

public class LayerChanger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 7) other.gameObject.layer = 8;

    }
}
