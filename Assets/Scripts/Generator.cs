using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

using Random = UnityEngine.Random;

public class Generator : MonoBehaviour, IGameLoopComponent
{
    [SerializeField] private TextMeshProUGUI _name;
    [SerializeField] private TextMeshProUGUI _reason;
    [SerializeField] private TextMeshProUGUI _nature;
    [SerializeField] private TextMeshProUGUI _age;
    [SerializeField] private TextMeshProUGUI _mouseCount;
    [SerializeField] private Image _image;
    private List<string> _catNames;
    private List<string> _catReasons;
    private List<string> _catNatures;
    private List<Image> _catImages;

    private ChartsParamsSO _chartsParamsSO;

    public event Action OnStartGeneration;

    public void StarLoop()
    {
        var allParameters = Resources.LoadAll<CatParameter>("");
        _catNames = allParameters[0].names;
        _catReasons = allParameters[0].reasons;
        _catNatures = allParameters[0].natures;
        _catImages = allParameters[0].images;

        _chartsParamsSO = allParameters[0].ChartsParams;

        GenerateCat();
    }

    public void GenerateCat()
    {
        OnStartGeneration?.Invoke();

        int randomNames = Random.Range(0, _catNames.Count);
        int randomReasons = Random.Range(0, _catReasons.Count);
        int randomNatures = Random.Range(0, _catNatures.Count);
        int randomImages = Random.Range (0, _catImages.Count);
        int randomAge = Random.Range(0, 20);
        int randomMouseCount = Random.Range(0, 5000);

        _name.text = _catNames[randomNames];
        _reason.text = _catReasons[randomReasons];
        _nature.text = _catNatures[randomNatures];
        //_image = _catImages[randomImages];
        _age.text = randomAge.ToString();
        _mouseCount.text = randomMouseCount.ToString();
    }

    public double GenerateRandomAmplitude()
    {
        var randomAmplitude = (double)(Random.Range(_chartsParamsSO.MinAmplitude, _chartsParamsSO.MaxAmplitude - 1) + Random.Range(0, 6) * 0.2f);
        return randomAmplitude;
    }

    public double GenerateRandomFrequency()
    {
        return (double)Random.Range(_chartsParamsSO.MinFrequency, _chartsParamsSO.MaxFrequency);
    }
}
