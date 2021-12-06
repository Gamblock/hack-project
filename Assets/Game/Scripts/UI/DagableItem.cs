using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DagableItem : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler
{
    public event Action<bool> OnItemDropped = b => { };
    public TypeEnums.ResourceTypes resourceType;
    public CurrentSelectedResourcesSO currentSelectedResourcesSo;
    public float scale = 1.5f;
    [SerializeField] private Canvas canvas;
    private CanvasGroup canvasGroup;
    private RectTransform rectTransform;
    private Vector3 startPosition;
    

    [HideInInspector] public bool inPlace;


    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        rectTransform = GetComponent<RectTransform>();
        startPosition = rectTransform.anchoredPosition;
    }

    public void OnPointerDown(PointerEventData eventdata)
    {
       Debug.Log("Pointer down");
    }

    public void OnEndDrag(PointerEventData eventDta)
    {
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1f;
        if (!inPlace)
        {
            rectTransform.anchoredPosition = startPosition;
        }
        else
        {
            currentSelectedResourcesSo.selectedResourcesList.Add(resourceType);
            rectTransform.localScale = new Vector3(scale, scale, scale);
            OnItemDropped.Invoke(true);
        }
    }
    
    public void OnBeginDrag(PointerEventData eventDta)
    {
        if (inPlace)
        {
            currentSelectedResourcesSo.selectedResourcesList.Remove(resourceType);
            OnItemDropped.Invoke(false);
        }
        inPlace = false;
        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;
        rectTransform.localScale = new Vector3(1, 1, 1);
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnDrop(PointerEventData eventData)
    {
        
    }
}
