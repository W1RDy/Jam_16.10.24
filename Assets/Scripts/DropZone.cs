using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DropZone : MonoBehaviour, IDropHandler
{
    [SerializeField] Generator _generator;
    [SerializeField] Sprite[] _seals;
    [SerializeField] Image _sealPlace;

    public void OnDrop(PointerEventData eventData)
    {
        GameObject droppedObject = eventData.pointerDrag;

        if (droppedObject.TryGetComponent(out LiveSeal liveSeal))
        {
            AddSeal(0);
            Invoke("DeleteSeal", 5f);              
        }

        if (droppedObject.TryGetComponent(out DeadSeal deadSeal))
        {
            int deaths = PlayerPrefs.GetInt("deaths");
            deaths += 1;
            PlayerPrefs.SetInt("deaths", deaths);

            AddSeal(1);
            Invoke("DeleteSeal", 3f);
        }
    }

    private void DeleteSeal()
    {
        _sealPlace.gameObject.SetActive(false);
        _generator.AddNewCat();
    }

    private void AddSeal(int number)
    {
        _sealPlace.gameObject.SetActive(true);
        _sealPlace.sprite = _seals[number];
    }
}
