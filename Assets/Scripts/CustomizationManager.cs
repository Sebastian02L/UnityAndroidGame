using TMPro;
using UnityEngine;

public class CustomizationManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _pointsText;
    [SerializeField] private TextMeshProUGUI _coinsText;

    private void OnEnable()
    {
        _pointsText.text = PlayerDataManager.Instance.MaxPoints.ToString() + "m";
        _coinsText.text = PlayerDataManager.Instance.Coins.ToString();
    }
}
