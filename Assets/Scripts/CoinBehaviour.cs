using UnityEngine;

public class CoinBehaviour : MonoBehaviour
{
    [SerializeField] private int _numCoins = 10;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        GameObject.FindAnyObjectByType<AudioManager>().PlayCoal();
        PlayerDataManager.Instance.AddCoins(_numCoins);
        Destroy(this.gameObject);
    }
}
