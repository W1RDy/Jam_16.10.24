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

    [SerializeField] private Generator _generator;

    public Wave Wave { get; private set; }

    private ChartsView _view;

    private bool _isDrawed;

    public event Action OnChartUpdated;
    public event Action<Wave> OnConstChartUpdated;
    public event Action OnChartGenerated;

    [SerializeField] private bool _isCanRegenerate;

    private void Awake()
    {
        _view = new ChartsView(_graphHandler);
        Subscribe();
    }

    public void UpdateChart(double frequency, double amplitude, int length, int phase = 0)
    {
        _frequency = frequency;
        _amplitude = Math.Clamp(amplitude, -3, 3);
        _length = length;
        _phase = phase;

        Wave = GenerateWave();

        UpdateChartView();
        OnChartUpdated?.Invoke();
    }

    private Wave GenerateWave()
    {
        return new Wave(_frequency, _amplitude, _length, _phase);
    }

    private void GenerateAllCharts()
    {
        GenerateConstChartValues();
        GenerateChart();
    }

    private void GenerateConstChartValues()
    {
        _amplitude = _generator.GenerateRandomAmplitude();
        _frequency = _generator.GenerateRandomFrequency();

        var wave = GenerateWave();

        OnConstChartUpdated?.Invoke(wave);
    }

    private void GenerateChart()
    {
        _amplitude = _generator.GenerateRandomAmplitude();

        Wave = GenerateWave();
        UpdateChartView();
        OnChartGenerated?.Invoke();
    }

    private void UpdateChartView()
    {
        _view.GenerateWaveDiagram(Wave);
    }

    private void ChangeAmplitude(float amplitude)
    {
        _amplitude += amplitude;
        UpdateChart(_frequency, _amplitude, _length);
    }

    private void Subscribe()
    {
        _generator.OnStartGeneration += GenerateAllCharts;
        
        if (_isCanRegenerate) _generator.OnStartRegeneration += GenerateChart;

        foreach (var button in _buttons)
        {
            button.OnChangeAmplitude += ChangeAmplitude;
        }
    }

    private void Unsubscribe()
    {
        _generator.OnStartGeneration -= GenerateAllCharts;

        if (_isCanRegenerate) _generator.OnStartRegeneration -= GenerateChart;

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
