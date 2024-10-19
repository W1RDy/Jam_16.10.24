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

    [SerializeField] private CoinsCounter _coinsCounter;

    public void OnDrop(PointerEventData eventData)
    {
        GameObject droppedObject = eventData.pointerDrag;

        if (droppedObject.TryGetComponent(out LiveSeal liveSeal) && !_generator.isChecked)
        {
            _anim.Play("leave");
            AddSeal(0);           
            Invoke("DeleteSeal", 1f);

            _coinsCounter.AddCoins(1);

            AudioManager.Instance.PlaySound("appear");
            AudioManager.Instance.PlaySound("stamp");
        }

        if (droppedObject.TryGetComponent(out DeadSeal deadSeal) && !_generator.isChecked)
        {
            int deaths = SaveSystem.Instance.SaveData.Deaths;
            deaths += 1;
            SaveSystem.Instance.SaveData.Deaths = deaths;

            _anim.Play("leave");
            AddSeal(1);            
            Invoke("DeleteSeal", 1f);

            _coinsCounter.AddCoins(1);

            AudioManager.Instance.PlaySound("appear");
            AudioManager.Instance.PlaySound("stamp");
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
