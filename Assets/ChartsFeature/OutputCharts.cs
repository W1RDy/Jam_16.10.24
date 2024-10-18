using System;
using UnityEngine;

public class OutputCharts : MonoBehaviour
{
    [SerializeField] protected Charts[] _charts;
    [SerializeField] protected GraphHandler _graphHandler;

    public event Action OnChartUpdated;

    public Wave Wave { get; protected set; }

    protected ChartsView _view;

    private void Awake()
    {
        _view = new ChartsView(_graphHandler);
        Subscribe();      
    }

    protected virtual void UpdateChart()
    {
        Wave = GenerateSumWave(_charts[0].Wave, _charts[1].Wave);
        if (Wave != null)
        {
            _view.GenerateWaveDiagram(Wave);
            CallEvent();
        }
    }

    protected void CallEvent()
    {
        OnChartUpdated?.Invoke();
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
        }

        return new Wave(points, new double[2] { wave1.Amplitudes[0], wave2.Amplitudes[0] }, new double[2] { wave1.Frequencies[0], wave2.Frequencies[0] }); 
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
