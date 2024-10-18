using System;
using UnityEngine;

public class ChangeAmplitudeButton : CustomButton
{
    [SerializeField] private float _amplitudeChangeValue;

    public event Action<float> OnChangeAmplitude; 

    protected override void OnClick()
    {
        OnChangeAmplitude?.Invoke(_amplitudeChangeValue);
    }
}