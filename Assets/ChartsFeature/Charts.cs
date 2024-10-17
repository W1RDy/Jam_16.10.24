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

    private ChartsView _view;

    private bool _isDrawed;

    private void Awake()
    {
        _view = new ChartsView(_graphHandler);
    }

    public void Update()
    {
        if (_graphHandler.IsPrepared)
        {
            if (!_isDrawed)
            {
                UpdateChart();
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

        UpdateChart();
    }

    private void UpdateChart()
    {
        _view.GenerateWaveDiagram(_frequency, _amplitude, _length, _phase);
    }
}
