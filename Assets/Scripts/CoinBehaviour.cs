using UnityEngine;

public class CoinBehaviour : MonoBehaviour
{
    [SerializeField] private int _numCoins = 10;
    UIManager _uiManager;

    private void Awake()
    {
        _uiManager = GameObject.FindAnyObjectByType<UIManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        GameObject.FindAnyObjectByType<AudioManager>().PlayCoal();
        PlayerDataManager.Instance.AddCoins(_numCoins);
        _uiManager.UpdateCoins(_numCoins);
        Destroy(this.gameObject);
    }
}
