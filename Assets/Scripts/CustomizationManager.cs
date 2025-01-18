using TMPro;
using UnityEngine;

public class CustomizationManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _pointsText;

    private void OnEnable()
    {
        _pointsText.text = PlayerDataManager.Instance.MaxPoints.ToString() + "m";
    }
}
