using System.Collections.Generic;

public class ConstOutputCharts : OutputCharts
{
    private List<Wave> _waves = new List<Wave>();

    private void AddConstValues(Wave wave)
    {
        _waves.Add(wave);

        if (_waves.Count == 2) UpdateChart();
    }

    protected override void UpdateChart()
    {
        if (_waves[0] == null || _waves[1] == null) return;

        var wave = GenerateSumWave(_waves[0], _waves[1]);
        if (wave != null) _view.GenerateWaveDiagram(wave);

        _waves.Clear();
    }

    protected override void Subscribe()
    {
        foreach (var chart in _charts)
        {
            chart.OnConstChartUpdated += AddConstValues;
        }
    }

    protected override void Unsubscribe()
    {
        foreach (var chart in _charts)
        {
            chart.OnConstChartUpdated -= AddConstValues;
        }
    }
}