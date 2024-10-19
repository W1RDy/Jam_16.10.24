using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneGoodsUpdater : MonoBehaviour
{
    [SerializeField] private SceneGoods[] _goods;

    private void Start()
    {
        UpdateGoods();
    }

    private void UpdateGoods()
    {
        var boughtGoodsIndexes = SaveSystem.Instance.SaveData.BoughtGoodsIndexes;

        foreach (var index in boughtGoodsIndexes)
        {
            ActivateGoods(index);
        }
    }

    public void ActivateGoods(int index)
    {
        _goods[index].ActivateGoods(); 
    }
}
