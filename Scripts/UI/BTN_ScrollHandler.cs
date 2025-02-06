using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BTN_ScrollHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private ScrollRect scrollRect;

    public void OnBeginDrag(PointerEventData e)
    {
        scrollRect.OnBeginDrag(e);
    }

    public void OnDrag(PointerEventData e)
    {
        scrollRect.OnDrag(e);
    }

    public void OnEndDrag(PointerEventData e)
    {
        scrollRect.OnEndDrag(e);
    }
}
