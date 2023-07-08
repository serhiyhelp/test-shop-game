using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DndItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private Image         _image;
    [SerializeField] private RectTransform _rectTransform;

    private Item _item;

    public Item Item
    {
        get => _item;
        set
        {
            _item = value;
            _image.sprite = Item.uiView;
        }
    }

    public Slot Container
    {
        get;
        set;
    }

    public void Init(Slot container, Item item)
    {
        Item      = item;
        Container = container;
    }
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        //Container = transform.parent;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        _image.raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(Container.rectTransform);
        _rectTransform.anchoredPosition = Vector2.zero;
        _image.raycastTarget            = true;
    }

    public void MoveToContainer()
    {
        transform.SetParent(Container.rectTransform);
        _rectTransform.anchoredPosition = Vector2.zero;
    }
}