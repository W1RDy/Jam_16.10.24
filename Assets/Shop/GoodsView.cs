using UnityEngine;

public class GoodsView
{
    private GameObject _goods;

    public GoodsView(GameObject goods) 
    {
        _goods = goods;
    }

    public void HideGoods()
    {
        _goods.SetActive(false);
    }
}