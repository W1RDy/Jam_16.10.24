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
    [SerializeField] private Animator _anim;

    public void OnDrop(PointerEventData eventData)
    {
        GameObject droppedObject = eventData.pointerDrag;

        if (droppedObject.TryGetComponent(out LiveSeal liveSeal) && !_generator.isChecked)
        {
            _anim.SetInteger("state", 0);
            AddSeal(0);           
            Invoke("DeleteSeal", 1f);              
        }

        if (droppedObject.TryGetComponent(out DeadSeal deadSeal) && !_generator.isChecked)
        {
            int deaths = PlayerPrefs.GetInt("deaths");
            deaths += 1;
            PlayerPrefs.SetInt("deaths", deaths);

            _anim.SetInteger("state", 0);
            AddSeal(1);            
            Invoke("DeleteSeal", 2f);
        }
    }

    private void DeleteSeal()
    {
        _generator.isChecked = false;
        _sealPlace.gameObject.SetActive(false);
        _generator.AddNewCat();
    }

    private void AddSeal(int number)
    {
        _sealPlace.gameObject.SetActive(true);
        _sealPlace.sprite = _seals[number];
        _generator.isChecked = true;
    }
}
