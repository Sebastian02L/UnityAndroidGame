using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CustomizationManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _pointsText;
    [SerializeField] private TextMeshProUGUI _coinsText;
    [SerializeField] private Button[] _buttons;
    [SerializeField] private GameObject _coin;
    private int _currentButton = 0;

    private void OnEnable()
    {
        _pointsText.text = PlayerDataManager.Instance.MaxPoints.ToString() + "m";
        _coinsText.text = PlayerDataManager.Instance.Coins.ToString();
        SetActiveButton(PlayerDataManager.Instance.ActiveSkin);
    }

    private void OnDisable()
    {
        _buttons[_currentButton].image.color = Color.white;
        _buttons[_currentButton].transform.Find("BuyButton").gameObject.SetActive(false);
        _buttons[_currentButton].transform.Find("EquipButton").gameObject.SetActive(false);
    }

    public void SetActiveButton(int index)
    {
        _buttons[_currentButton].image.color = Color.white;
        _buttons[_currentButton].transform.Find("BuyButton").gameObject.SetActive(false);
        _buttons[_currentButton].transform.Find("EquipButton").gameObject.SetActive(false);
        _currentButton = index;
        _buttons[_currentButton].image.color = Color.gray;
        Button buyButton = _buttons[_currentButton].transform.Find("BuyButton").gameObject.GetComponent<Button>();
        Button equipButton = _buttons[_currentButton].transform.Find("EquipButton").gameObject.GetComponent<Button>();
        if (PlayerDataManager.Instance.Skins[_currentButton])
        {
            equipButton.gameObject.SetActive(true);
            _coin.SetActive(false);
            if (PlayerDataManager.Instance.ActiveSkin == _currentButton)
            {
                equipButton.interactable = false;
                equipButton.transform.GetComponentInChildren<TextMeshProUGUI>().text = "Equipada";
            }
            else
            {
                equipButton.interactable = true;
                equipButton.transform.GetComponentInChildren<TextMeshProUGUI>().text = "Equipar";
            }
        } 
        else
        {
            buyButton.gameObject.SetActive(true);
            _coin.SetActive(true);
            if (PlayerDataManager.Instance.Coins - (300 * index + 500 * (index - 1) + 100) < 0)
            {
                buyButton.transform.GetComponentInChildren<TextMeshProUGUI>().color = Color.red;
                buyButton.interactable = false;
            }
            else
            {
                buyButton.transform.GetComponentInChildren<TextMeshProUGUI>().color = Color.black;
                buyButton.interactable = true;
            }
        }
    }

    public void BuySkin(int skinIndex)
    {
        int skinPrice = 300 * skinIndex + 500 * (skinIndex - 1) + 100;
        PlayerDataManager.Instance.AddCoins(-skinPrice);
        _coinsText.text = PlayerDataManager.Instance.Coins.ToString();
        List<bool> skins = PlayerDataManager.Instance.Skins;
        skins[skinIndex] = true;
        PlayerDataManager.Instance.SetSkins(skinIndex, true);
        SetActiveButton(skinIndex);
    }

    public void EquipSkin(int activeSkin)
    {
        PlayerDataManager.Instance.SetActiveSkin(activeSkin);
        SetActiveButton(activeSkin);
    }
}
