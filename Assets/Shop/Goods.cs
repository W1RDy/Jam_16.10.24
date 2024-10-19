using UnityEngine;

public class Goods : MonoBehaviour
{
    [SerializeField] private int _price;

    [SerializeField] private GoodsButton _button;

    [SerializeField] private SceneGoodsUpdater _goodsUpdater;
    [SerializeField] private CoinsCounter _coinsCounter;

    public int Index { get; private set; }

    private GoodsView _view;

    private bool _isBought;

    public void Init(int index, bool isBought)
    {
        Index = index;
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
        var currentCoins = _coinsCounter.GetCurrentCoins();

        if (currentCoins >= _price)
        {
            _coinsCounter.RemoveCoins(_price);
            SaveSystem.Instance.SaveData.BoughtGoodsIndexes.Add(Index);
            _view.HideGoods();

            _goodsUpdater.ActivateGoods(Index);
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
