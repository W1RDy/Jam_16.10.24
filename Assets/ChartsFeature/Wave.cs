using System;
using System.Linq;
using UnityEngine;

public class Wave
{
    public Vector2[] Points { get; private set; }

    public float MinYValue { get; private set; }
    public float MaxYValue { get; private set; }

    public float MinXValue { get; private set; }
    public float MaxXValue { get; private set; }

    public Wave(double frequency, double amplitude, int length, int phase = 0)
    {
        GenerateWave(frequency, amplitude, length, phase);
    }

    public Wave(Vector2[] points)
    {
        Points = points;
    }

    private void GenerateWave(double frequency, double amplitude, int length, int phase = 0)
    {
        var x = Enumerable.Range(0, length).Select(i => i * 2 * Math.PI / length).ToArray();
        var y = x.Select(i => amplitude * Math.Sin(frequency * i + phase)).ToArray();

        MaxXValue = (float)x[x.Length - 1];
        MinXValue = (float)x[0];

        Points = new Vector2[x.Length];

        for (int i = 0; i < x.Length; i++)
        {
            var point = new Vector2((float)x[i], (float)y[i]);
            Points[i] = point;

            if (point.y < MinYValue) { MinYValue = point.y; }
            if (point.y > MaxYValue) { MaxYValue = point.y; }
        }
    }

    public void RegenerateWave(double frequency, double amplitude, int length, int phase = 0)
    {
        GenerateWave(frequency, amplitude, length, phase);
    }
}