using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropZone : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        GameObject droppedObject = eventData.pointerDrag;

        if (droppedObject.TryGetComponent(out LiveSeal liveSeal))
        {
            print("Жив!");
        }

        if (droppedObject.TryGetComponent(out DeadSeal deadSeal))
        {
            print("Мертв!");
        }
    }
}
