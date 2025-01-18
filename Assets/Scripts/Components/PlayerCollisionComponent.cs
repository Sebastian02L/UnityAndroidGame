using UnityEngine;

public class PlayerCollisionComponent : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if(!other.CompareTag("Coin")) GameObject.FindAnyObjectByType<GameManager>().EndGame();
    }
}
