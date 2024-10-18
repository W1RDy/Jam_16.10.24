using System;
using UnityEngine;

public class ChartsChecker : MonoBehaviour
{
    [SerializeField] private OutputCharts[] _charts;

    public event Action OnChartsEquals;

    private void Awake()
    {
        Subscribe();
    }

    private void CheckCharts()
    {
        if (_charts[0].Wave == null || _charts[1].Wave == null) return;

        if (CheckEqualsParamsWithOffset(_charts[0].Wave.Points, _charts[1].Wave.Points, 0.2f))
        {
            ChartsFinished();
        }
    }

    private bool CheckEqualsParamsWithOffset(Vector2[] params1, Vector2[] params2, double offset)
    {
        if (params1.Length != params2.Length) return false;

        Debug.LogWarning("Проверка графов");
        for (int i = 0; i < params1.Length; ++i)
        {
            Debug.Log($"Ожидалось ({params1[i].x}, {params1[i].y}), получено ({params2[i].x}, {params2[i].y})");
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
            chart.OnChartUpdated += CheckCharts;
        }
    }

    private void Unsubscribe()
    {
        foreach (var chart in _charts)
        {
            chart.OnChartUpdated -= CheckCharts;
        }
    }

    private void OnDestroy()
    {
        Unsubscribe();
    }
}