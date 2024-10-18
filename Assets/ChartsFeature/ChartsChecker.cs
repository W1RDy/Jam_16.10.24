using System;
using UnityEngine;

public class ChartsChecker : MonoBehaviour
{
    [SerializeField] private OutputCharts[] _charts;

    [SerializeField] private Generator _generator; 

    public event Action OnChartsEquals;

    private void Awake()
    {
        Subscribe();
    }

    private void CheckGeneratedCharts()
    {
        if (_charts[0].Wave == null || _charts[1].Wave == null) return;

        if (!CheckMoreEqualsParamsWithOffset(_charts[0].Wave.Points, _charts[1].Wave.Points, 0.6f))
        {
            _generator.RegenerateGraphs();
        }
    }

    private void CheckUpdatedCharts()
    {
        if (_charts[0].Wave == null || _charts[1].Wave == null) return;

        if (CheckEqualsParamsWithOffset(_charts[0].Wave.Points, _charts[1].Wave.Points, 0.4f))
        {
            ChartsFinished();
        }
    }

    private bool CheckMoreEqualsParamsWithOffset(Vector2[] params1,  Vector2[] params2, double offset)
    {
        if (params1.Length != params2.Length) return false;

        int equalsCount = 0;

        for (int i = 0; i < params1.Length; ++i)
        {
            if (!CheckEqualsParamsWithOffset(params1[i], params2[i], offset))
            {
                equalsCount++;
            }
        }

        return equalsCount > params1.Length / 2;
    }

    private bool CheckEqualsParamsWithOffset(Vector2[] params1, Vector2[] params2, double offset)
    {
        if (params1.Length != params2.Length) return false;

        for (int i = 0; i < params1.Length; ++i)
        {
            if (!CheckEqualsParamsWithOffset(params1[i], params2[i], offset))
            {
                return false;
            }
        }

        return true;
    }

    private bool CheckEqualsParamsWithOffset(Vector2 param1, Vector2 param2, double offset)
    {
        return Math.Abs(Vector2.Distance(param1, param2)) < offset;
    }

    private void ChartsFinished()
    {
        OnChartsEquals?.Invoke();
    }

    private void Subscribe()
    {
        foreach (var chart in _charts)
        {
            chart.OnChartUpdated += CheckUpdatedCharts;
            chart.OnChartGenerated += CheckGeneratedCharts;
        }
    }

    private void Unsubscribe()
    {
        foreach (var chart in _charts)
        {
            chart.OnChartUpdated -= CheckUpdatedCharts;
            chart.OnChartGenerated -= CheckGeneratedCharts;
        }
    }

    private void OnDestroy()
    {
        Unsubscribe();
    }
}