using System.Collections;
using UnityEngine;

public class PlayerCollisionComponent : MonoBehaviour
{
    [SerializeField] private GameObject _explosion;
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Coin"))
        {
            GetComponent<Collider>().enabled = false;
            StartCoroutine(EndGame());
            _explosion.SetActive(true);
        }
    }

    IEnumerator EndGame()
    {
        GameObject.FindAnyObjectByType<GameManager>().SetGameState(false);
        GameObject.FindAnyObjectByType<AudioManager>().PlayDeath();
        yield return new WaitForSeconds(0.5f);
        GameObject.FindAnyObjectByType<GameManager>().EndGame();
    }
}
