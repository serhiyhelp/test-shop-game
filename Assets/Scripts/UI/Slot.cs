using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour, IDropHandler
{
    [SerializeField] private GameObject    placeholder;
    [SerializeField] private RectTransform rectTransform;
    
    public DndItem   dndPrefab;
    public ClothType Filter;
    
    public UnityEvent<Item, Slot> contentChanged;

    private DndItem _content;

    public DndItem Content
    {
        get => _content;
        set
        {
            _content = value;
            contentChanged?.Invoke(value?.Item, this);
        }
    }

    public int Id
    {
        get;
        private set;
    }

    public bool IsOwnedByMerchant
    {
        get;
        private set;
    }

    public RectTransform RectTransform
    {
        get => rectTransform;
        set => rectTransform = value;
    }


    public void Init(int id, Item item, bool isOwnedByMerchant = false)
    {
        Id                = id;
        IsOwnedByMerchant = isOwnedByMerchant;
        if (item != null)
        {
            var dndItem = Instantiate(dndPrefab, rectTransform);
            dndItem.Init(this, item, IsOwnedByMerchant);
            Content = dndItem;
            if (placeholder) placeholder.SetActive(false);
        }
    }

    public void AcceptItem(Item item)
    {
        if (item)
        {
            if (Content == null)
            {
                var dndItem = Instantiate(dndPrefab, rectTransform);
                dndItem.Init(this, item, IsOwnedByMerchant);
                Content = dndItem;
                if (placeholder) placeholder.SetActive(false);
            }
            else
            {
                Content.Item = item;
            }
        }
        else
        {
            if (Content)
            {
                Destroy(Content.gameObject);
                Content = null;
                if (placeholder) placeholder.SetActive(true);
            }
        }
    }

    private void ContentChangedNotify()
    {
        contentChanged.Invoke(Content?.Item, this);
        if (placeholder) placeholder.SetActive(!Content);
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag.TryGetComponent<DndItem>(out var dropped) && (Filter == ClothType.None || Filter == dropped.Item.type))
        {
            var oppositeSlot = dropped.Container;
            if (oppositeSlot == this) return;

            oppositeSlot.Content = this.Content;
            if (Content != null)
            {
                Content.Container = oppositeSlot;
                this.Content.MoveToContainer();
            }
            
            //oppositeSlot.ContentChangedNotify();

            this.Content      = dropped;
            dropped.Container = this;
            dropped.MoveToContainer();
            //ContentChangedNotify();
            
            GlobalEvents.InterSlotExchange.Invoke(this, oppositeSlot);
        }
    }

    private void OnValidate()
    {
        if (!RectTransform)
        {
            RectTransform = GetComponent<RectTransform>();
        }
    }
}