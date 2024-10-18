using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Generator : MonoBehaviour
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

    // Start is called before the first frame update
    void Start()
    {
        var allParameters = Resources.LoadAll<CatParameter>("");
        _catNames =  allParameters[0].names;
        _catReasons = allParameters[0].reasons;
        _catNatures = allParameters[0].natures;
        _catImages = allParameters[0].images;

        AddNewCat();
    }

    public void AddNewCat()
    {
        int randomImages = Random.Range(0, _catImages.Count);
        //_image = _catImages[randomImages];
        CleanCat();
    }

    public void GenerateCat()
    {
        int randomNames = Random.Range(0, _catNames.Count);
        int randomReasons = Random.Range(0, _catReasons.Count);
        int randomNatures = Random.Range(0, _catNatures.Count);        
        int randomAge = Random.Range(0, 20);
        int randomMouseCount = Random.Range(0, 5000);

        _name.text = _catNames[randomNames];
        _reason.text = _catReasons[randomReasons];
        _nature.text = _catNatures[randomNatures];        
        _age.text = randomAge.ToString();
        _mouseCount.text = randomMouseCount.ToString();
    }

    private void CleanCat()
    {
        string empty = "...";
        _name.text = empty;
        _reason.text = empty;
        _nature.text = empty;
        _age.text = empty;
        _mouseCount.text = empty;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GenerateCat();
        }
    }

}
