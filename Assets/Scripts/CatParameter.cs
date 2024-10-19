using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "AllParameter", menuName = "Cat/Parameter")]
public class CatParameter : ScriptableObject
{
    public List<string> names;
    public List<string> reasons;
    public List<string> natures;
    public List<Sprite> images;

    [SerializeField] private ChartsParamsSO _chartsParams;

    public ChartsParamsSO ChartsParams => _chartsParams;
}
