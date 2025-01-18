using System.Collections;
using UnityEngine;

public class PlayerCollisionComponent : MonoBehaviour
{
    [SerializeField] private ParticleSystem _explosion;
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Coin"))
        {
            StartCoroutine(EndGame());
            _explosion.Play();
        }
    }

    IEnumerator EndGame()
    {
        yield return new WaitForSeconds(1.5f);
        GameObject.FindAnyObjectByType<GameManager>().EndGame();
    }
}
