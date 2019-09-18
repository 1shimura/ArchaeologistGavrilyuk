using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TreasureCell : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler
{
    private RectTransform _canvasTransform;
    private RectTransform _rectTransform;
    private RectTransform _treasureGroupTransform;

    public event Action<PointerEventData, TreasureCell> DragEnd;

    private Vector2 _dragStartPosition;

    public void Init(RectTransform canvasTransform, Sprite sprite, RectTransform treasureGroupTransform)
    {
        _canvasTransform = canvasTransform;
        _rectTransform = GetComponent<RectTransform>();
        _treasureGroupTransform = treasureGroupTransform;

        GetComponent<Image>().sprite = sprite;
    }
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        _dragStartPosition = _rectTransform.localPosition;
        _rectTransform.SetParent(_canvasTransform);
    }
    
    public void OnDrag(PointerEventData eventData)
    {
        _rectTransform.localPosition = CameraToCanvasPosition(eventData.position);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _rectTransform.SetParent(_treasureGroupTransform);
        DragEnd?.Invoke(eventData, this);
    }

    public void ResetPosition()
    {
        _rectTransform.localPosition = _dragStartPosition;
    }

    private Vector2 CameraToCanvasPosition(Vector2 cameraPosition)
    {
        return cameraPosition / new Vector2(Screen.width, Screen.height) * _canvasTransform.sizeDelta - _canvasTransform.sizeDelta * .5f;
    }
}
