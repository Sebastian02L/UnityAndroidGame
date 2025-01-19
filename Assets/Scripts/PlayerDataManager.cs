using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerDataManager : MonoBehaviour
{
    public static PlayerDataManager Instance { get; private set; }

    public int Coins { get; private set; } = 0;
    public int MaxPoints { get; private set; } = 0;
    public List<bool> Skins { get; private set; } = new List<bool>();
    public int ActiveSkin { get; private set; } = 0;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this.gameObject);
        DontDestroyOnLoad(this.gameObject);
        CreateDataSystem();
    }

    public void CreateDataSystem()
    {
        Coins = PlayerPrefs.GetInt("coins", 0);
        MaxPoints = PlayerPrefs.GetInt("points", 0);
        ActiveSkin = PlayerPrefs.GetInt("activeSkin", 0);
        List<string> skinList = PlayerPrefs.GetString("skins", "true.false.false").Split(".").ToList();
        foreach (string skin in skinList)
        {
            Skins.Add(bool.Parse(skin));
        }
    }

    public void SetCoins(int amount)
    {
        Coins = amount;
        PlayerPrefs.SetInt("coins", Coins);
        SaveData();
    }

    public bool AddCoins(int amount)
    {
        if (Coins + amount < 0) return false;
        SetCoins(Coins + amount);
        return true;
    }

    public void SetPoints(int amount)
    {
        if (MaxPoints > amount) return;
        MaxPoints = amount;
        PlayerPrefs.SetInt("points", MaxPoints);
        SaveData();
    }

    public void SetSkins(int index, bool b)
    {
        Skins[index] = b;
        string joinedString = string.Join(".", Skins);
        PlayerPrefs.SetString("skins", joinedString);
        SaveData();
    }

    public void SetActiveSkin(int activeSkin)
    {
        ActiveSkin = activeSkin;
        PlayerPrefs.SetInt("activeSkin", ActiveSkin);
        SaveData();
    }

    public void SaveData()
    {
        PlayerPrefs.Save();
    }

    public void DeleteData()
    {
        PlayerPrefs.DeleteAll();
    }
}
