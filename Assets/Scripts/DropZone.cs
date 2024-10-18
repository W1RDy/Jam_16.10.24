using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DropZone : MonoBehaviour, IDropHandler
{
    [SerializeField] private Generator _generator;
    [SerializeField] private Sprite[] _seals;
    [SerializeField] private Image _sealPlace;

    [SerializeField] private bool checkBool = true;

    public void OnDrop(PointerEventData eventData)
    {
        GameObject droppedObject = eventData.pointerDrag;

        if (droppedObject.TryGetComponent(out LiveSeal liveSeal) && checkBool)
        {
            AddSeal(0);           
            Invoke("DeleteSeal", 5f);              
        }

        if (droppedObject.TryGetComponent(out DeadSeal deadSeal) && checkBool)
        {
            int deaths = PlayerPrefs.GetInt("deaths");
            deaths += 1;
            PlayerPrefs.SetInt("deaths", deaths);

            AddSeal(1);            
            Invoke("DeleteSeal", 2f);
        }
    }

    private void DeleteSeal()
    {
        checkBool = true;
        _sealPlace.gameObject.SetActive(false);
        _generator.AddNewCat();
    }

    private void AddSeal(int number)
    {
        _sealPlace.gameObject.SetActive(true);
        _sealPlace.sprite = _seals[number];
        checkBool = false;
    }
}
