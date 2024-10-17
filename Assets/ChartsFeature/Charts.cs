using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class Charts : MonoBehaviour
{
    [SerializeField] private double _frequency;
    [SerializeField] private double _amplitude;
    [SerializeField] private int _length;
    [SerializeField] private int _phase;

    [SerializeField] private GraphHandler _graphHandler;

    [SerializeField] private ChangeAmplitudeButton[] _buttons;

    public Wave Wave { get; private set; }

    private ChartsView _view;

    private bool _isDrawed;

    public event Action OnChartUpdated;

    private void Awake()
    {
        _view = new ChartsView(_graphHandler);
        Subscribe();
    }

    public void Update()
    {
        if (_graphHandler.IsPrepared)
        {
            if (!_isDrawed)
            {
                UpdateChart(_frequency, _amplitude, _length);
                _isDrawed = true;
            }
        }
    }

    public void UpdateChart(double frequency, double amplitude, int length, int phase = 0)
    {
        _frequency = frequency;
        _amplitude = amplitude;
        _length = length;
        _phase = phase;

        Wave = GenerateWave();

        UpdateChart();
    }

    private Wave GenerateWave()
    {
        return new Wave(_frequency, _amplitude, _length, _phase);
    }

    private void UpdateChart()
    {
        _view.GenerateWaveDiagram(Wave);
        OnChartUpdated?.Invoke();
    }

    private void ChangeAmplitude(float amplitude)
    {
        _amplitude = amplitude;
        UpdateChart(_frequency, _amplitude, _length);
    }

    private void Subscribe()
    {
        foreach (var button in _buttons)
        {
            button.OnChangeAmplitude += ChangeAmplitude;
        }
    }

    private void Unsubscribe()
    {
        foreach (var button in _buttons)
        {
            button.OnChangeAmplitude -= ChangeAmplitude;
        }
    }

    private void OnDestroy()
    {
        Unsubscribe();
    }
}
