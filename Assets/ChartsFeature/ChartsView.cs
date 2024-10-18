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

    //public void GenerateWaveDiagram(double frequency, double amplitude, int length, int phase = 0)
    //{

    //    GenerateWaveDiagram(wave);
    //}

    public void GenerateWaveDiagram(Wave wave)
    {
        _graphHandler.SetWave(wave);
    }
}
