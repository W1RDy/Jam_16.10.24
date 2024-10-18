using System;

public class GoodsButton : CustomButton
{
    public event Action OnBought; 

    protected override void OnClick()
    {
        OnBought?.Invoke();
    }
}
