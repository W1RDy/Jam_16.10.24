using System;
using UnityEngine;

public class ChangeAmplitudeButton : CustomButton
{
    [SerializeField] private float _amplitudeChangeValue;

    public event Action<float> OnChangeAmplitude; 

    protected override void OnClick()
    {
        AudioManager.Instance.PlaySound("click");
        OnChangeAmplitude?.Invoke(_amplitudeChangeValue);
    }
}