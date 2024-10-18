using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropZone : MonoBehaviour, IDropHandler
{
    [SerializeField] Generator _generator;

    public void OnDrop(PointerEventData eventData)
    {
        GameObject droppedObject = eventData.pointerDrag;

        if (droppedObject.TryGetComponent(out LiveSeal liveSeal))
        {
            _generator.AddNewCat();
            print("Жив!");            
        }

        if (droppedObject.TryGetComponent(out DeadSeal deadSeal))
        {
            _generator.AddNewCat();
            print("Мертв!");
        }
    }
}
