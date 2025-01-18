using UnityEngine;

public class CoinBehaviour : MonoBehaviour
{
    [SerializeField] private int _numCoins = 10;

    private void OnTriggerEnter()
    {
        //Play SFX
        PlayerDataManager.Instance.AddCoins(_numCoins);
        Destroy(this.gameObject);
    }
}
