using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChartsView
{


    private GraphHandler _graphHandler;

    public ChartsView(GraphHandler graphHandler)
    {
        _graphHandler = graphHandler; 
    }

    public void GenerateWaveDiagram(double frequency, double amplitude, int length, int phase = 0)
    {
        var wave = new Wave(frequency, amplitude, length, phase);
        _graphHandler.SetWave(wave);
    }
}
