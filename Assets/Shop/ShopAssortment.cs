using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopAssortment : MonoBehaviour
{
    [SerializeField] private Goods[] _goods;
    private bool _isUpdated;

    private void OnEnable()
    {
        if (!_isUpdated) UpdateAssortment();
    }

    private void UpdateAssortment()
    {
        for (int i = 0; i < _goods.Length; ++i)
        {
            bool isBought = false;

            if (SaveSystem.Instance.SaveData.BoughtGoodsIndexes.Count > 0)
            {
                isBought = SaveSystem.Instance.SaveData.BoughtGoodsIndexes.Contains(i);
            }

            _goods[i].Init(i, isBought);
        }
    }
}
