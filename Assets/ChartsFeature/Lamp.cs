using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class Lamp : MonoBehaviour
{
    [SerializeField] private ChartsChecker _chartsChecker;
    [SerializeField] private Generator _generator;

    [Space]
    [SerializeField] private Sprite _commonLight;
    [SerializeField] private Sprite _greenLight;
    [SerializeField] private Sprite _redLight;

    private bool _isActivated;

    private Image _renderer;

    private void Awake()
    {
        _renderer = GetComponent<Image>();
        Subscribe();
    }

    private void ActivateLight()
    {
        if (!_isActivated)
        {
            _isActivated = true;
            var lifeState = _generator.GenerateLifeState();
            if (lifeState) TurnOnPositiveLight();
            else TurnOnNegativeLight();
        }
    }

    private void TurnOnPositiveLight()
    {
        _renderer.sprite = _greenLight;
    }

    private void TurnOnNegativeLight()
    {
        _renderer.sprite = _redLight;
    }

    private void TurnOffLight()
    {
        _renderer.sprite = _commonLight;
        _isActivated = false;
    }

    private void Subscribe()
    {
        _chartsChecker.OnChartsEquals += ActivateLight;
        _generator.OnStartGeneration += TurnOffLight;
    }

    private void Unsubscribe()
    {
        _chartsChecker.OnChartsEquals -= ActivateLight;
        _generator.OnStartGeneration -= TurnOffLight;
    }

    private void OnDestroy()
    {
        Unsubscribe();
    }
}
