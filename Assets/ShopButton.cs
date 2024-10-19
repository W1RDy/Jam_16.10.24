using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopButton : CustomButton
{
    protected override void OnClick()
    {
        AudioManager.Instance.PlaySound("coinDrop");
    }
}
