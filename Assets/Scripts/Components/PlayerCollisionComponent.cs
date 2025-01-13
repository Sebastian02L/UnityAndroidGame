using UnityEngine;

public class PlayerCollisionComponent : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        GameObject.FindAnyObjectByType<GameManager>().EndGame();
    }
}
