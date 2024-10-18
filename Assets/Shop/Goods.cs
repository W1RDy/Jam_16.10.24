using UnityEngine;

public class Goods : MonoBehaviour
{
    [SerializeField] private int _price;

    [SerializeField] private GoodsButton _button;

    private int _index;

    private GoodsView _view;

    private bool _isBought;

    public void Init(int index, bool isBought)
    {
        _index = index;
        _isBought = isBought;

        _view = new GoodsView(gameObject);

        if (isBought)
        {
            _view.HideGoods();
        }
        else Subscribe();
    }

    public void Buy()
    {
        var currentCoins = SaveSystem.Instance.SaveData.CoinsCount;

        if (currentCoins >= _price)
        {
            SaveSystem.Instance.SaveData.CoinsCount -= _price;
            SaveSystem.Instance.SaveData.BoughtGoodsIndexes.Add(_index);
            _view.HideGoods();

            _isBought = true;
            Unsubscribe();
        }

    }

    public void Subscribe()
    {
        _button.OnBought += Buy;
    }

    public void Unsubscribe()
    {
        _button.OnBought -= Buy;
    }

    public void OnDestroy()
    {
        if (!_isBought)
        {
            Unsubscribe();
        }
    }
}
