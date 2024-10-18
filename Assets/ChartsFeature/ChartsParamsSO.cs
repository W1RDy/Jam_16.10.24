using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ChartsParams", menuName = "ChartsParams")]
public class ChartsParamsSO : ScriptableObject
{
    [SerializeField] private float _minFrequency;
    [SerializeField] private float _maxFrequency;
                             
    [Space]                  
    [SerializeField] private float _minAmplitude;
    [SerializeField] private float _maxAmplitude;

    public float MaxFrequency => _maxFrequency;
    public float MinFrequency => _minFrequency;

    public float MinAmplitude => _minAmplitude;
    public float MaxAmplitude => _maxAmplitude;
}
