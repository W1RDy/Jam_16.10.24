using System;
using UnityEngine;

public class OutputCharts : MonoBehaviour
{
    [SerializeField] protected Charts[] _charts;
    [SerializeField] protected GraphHandler _graphHandler;

    protected ChartsView _view;

    private void Awake()
    {
        _view = new ChartsView(_graphHandler);
        Subscribe();      
    }

    protected virtual void UpdateChart()
    {
        var wave = GenerateSumWave(_charts[0].Wave, _charts[1].Wave);
        if (wave != null) _view.GenerateWaveDiagram(wave);
    }

    protected Wave GenerateSumWave(Wave wave1, Wave wave2)
    {
        int length = 0;

        if (wave1 == null || wave2 == null) return null;

        if (wave1.Points.Length == wave2.Points.Length) length = wave1.Points.Length;
        else throw new Exception("Waves has different point counts");

        Vector2[] points = new Vector2[length];
        for (int i = 0; i < length; ++i)
        {
            points[i] = wave1.Points[i] + wave2.Points[i];
            Debug.Log(points[i]);
        }

        return new Wave(points); 
    }

    protected virtual void Subscribe()
    {
        foreach (var chart in _charts)
        {
            chart.OnChartUpdated += UpdateChart;
        }
    }

    protected virtual void Unsubscribe()
    {
        foreach (var chart in _charts)
        {
            chart.OnChartUpdated -= UpdateChart;
        }
    }

    private void OnDestroy()
    {
        Unsubscribe();
    }
}
