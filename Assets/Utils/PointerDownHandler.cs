using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class PointerDownHandler : EventTrigger
{
    public UnityEvent onPointerDown;

    public override void OnPointerDown(PointerEventData eventData)
    {
        onPointerDown?.Invoke();
    }
}
