using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    [SerializeField] private GameObject _main;
    [SerializeField] private GameObject _settings;
    [SerializeField] private GameObject _authors;
    
    void Start()
    {
        _main.SetActive(true);
        _settings.SetActive(false);
        _authors.SetActive(false);
    }

    public void ChangeSettings()
    {
        _main.SetActive(!_main.activeSelf);
        _settings.SetActive(!_main.activeSelf);
    }
    public void ChangeAuthors()
    {
        _main.SetActive(!_main.activeSelf);
        _authors.SetActive(!_authors.activeSelf);
    }


    void Update()
    {
        
    }
}
