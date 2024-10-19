using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CoinsCounter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;
    private int _coins;

    private void Start()
    {
        _coins = SaveSystem.Instance.SaveData.CoinsCount;
        UpdateText(_coins);
    }

    public void AddCoins(int coinsCount)
    {
        _coins += coinsCount;
        SaveSystem.Instance.SaveData.CoinsCount = _coins;

        UpdateText(_coins);
    }

    public void RemoveCoins(int coinsCount)
    {
        _coins = Math.Clamp(_coins - coinsCount, 0, _coins);
        SaveSystem.Instance.SaveData.CoinsCount = _coins;

        UpdateText(_coins);
    }

    private void UpdateText(int coins)
    {
        _text.text = "Монетки: " + coins.ToString();
    }

    public int GetCurrentCoins()
    {
        return _coins;
    }
}
